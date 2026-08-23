using AlluLoader;
using AlluLoader.Logging;
using Allumeria;
using System.Reflection;
using System.Runtime.Loader;
using HarmonyLib;

public sealed class Loader : IExternalLoader
{
    private static int _initialized;
    public static void Init()
    {
        if (Interlocked.Exchange(ref _initialized, 1) != 0) return;

        try
        {
            AlluLoader.Paths.CreateDirectories();
            AssemblyLoadContext.Default.Resolving += AlluLoader.SharedLibraryResolver.Resolve;
            InitializeApi();
            Log.Write($"AlluLoader started. " + $"Process='{Environment.ProcessPath ?? "unknown"}', " + $"PID={Environment.ProcessId}, " + $".NET='{Environment.Version}'");
            AlluLoader.ModLoader.LoadAll();
        }
        catch (Exception exception)
        {
            Log.Write($"Fatal loader error:{Environment.NewLine}{exception}");
        }
    }

    private static void InitializeApi()
    {
        ApiInitializer.Initialize();
        Log.Write("AlluLoader API initialized successfully.");
    }
}

namespace AlluLoader
{
    internal static class Paths
    {
        public static string Root { get; } = Path.Combine(AppContext.BaseDirectory, "mods");
        public static string Mods { get; } = Path.Combine(Root, "Mods");
        public static string Libraries { get; } = Path.Combine(Root, "Libraries");
        public static string Config { get; } = Path.Combine(Root, "Config");
        public static string Logs { get; } = Path.Combine(Root, "Logs");
        public static string LogFile { get; } = Path.Combine(Logs, "alluloader.log");
        public static void CreateDirectories()
        {
            foreach (string directory in new[] { Mods, Libraries, Config, Logs }) Directory.CreateDirectory(directory);
        }
    }

    internal static class ModLoader
    {
        private static readonly StringComparer PathComparer = OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;

        public static void LoadAll()
        {
            string[] modPaths = FindModAssemblies();
            Log.Write($"Found {modPaths.Length} mod(s).");
            foreach (string modPath in modPaths) LoadMod(modPath);
        }

        private static string[] FindModAssemblies()
        {
            var results = new HashSet<string>(PathComparer);
            foreach (string path in Directory.EnumerateFiles(Paths.Mods, "*.dll", SearchOption.TopDirectoryOnly))
            {
                results.Add(Path.GetFullPath(path));
            }
            foreach (string directory in Directory.EnumerateDirectories(Paths.Mods, "*", SearchOption.TopDirectoryOnly))
            {
                string modName = Path.GetFileName(directory);
                string mainAssembly = Path.Combine(directory, $"{modName}.dll");

                if (File.Exists(mainAssembly)) { results.Add(Path.GetFullPath(mainAssembly)); }
                else { Log.Write($"Skipped '{directory}': expected '{modName}.dll'."); }
            }
            return results.OrderBy(static path => path, PathComparer).ToArray();
        }

        private static void LoadMod(string modPath)
        {
            Harmony? harmony = null;
            try
            {
                Log.Write($"Loading mod: {modPath}");
                var loadContext = new ModLoadContext(modPath);
                Assembly assembly = loadContext.LoadFromAssemblyPath(modPath);
                string assemblyName = assembly.GetName().Name ?? "UnknownMod";
                string harmonyId = $"alluloader.{assemblyName.ToLowerInvariant()}." + assembly.ManifestModule.ModuleVersionId.ToString("N");
                harmony = new Harmony(harmonyId);
                harmony.PatchAll(assembly);
                InitializeModEntryPoints(assembly);
                Log.Write($"Loaded mod: {assemblyName}");
            }
            catch (Exception exception)
            {
                if (harmony is not null)
                {
                    try { harmony.UnpatchAll(harmony.Id); }
                    catch (Exception rollbackException) { Log.Write($"Patch rollback failed: {rollbackException}"); }
                }
                Log.Write($"Failed to load '{modPath}':" + $"{Environment.NewLine}{exception}");
            }
        }

        private static void InitializeModEntryPoints(Assembly assembly)
        {
            Type interfaceType = AssemblyLoadContext.Default.Assemblies.Select(a => a.GetType("AlluLoader.IMod")).FirstOrDefault(t => t is not null) ?? throw new TypeLoadException("AlluLoader.IMod is not loaded.");
            Type[] entryPointTypes = assembly.GetTypes().Where(type => type is { IsAbstract: false, IsInterface: false } && interfaceType.IsAssignableFrom(type)).ToArray();
            if (entryPointTypes.Length == 0)
            {
                throw new InvalidOperationException($"'{assembly.FullName}' contains no IMod entry point.");
            }
            foreach (Type entryPointType in entryPointTypes)
            {
                object instance = Activator.CreateInstance(entryPointType) ?? throw new InvalidOperationException($"Could not instantiate '{entryPointType.FullName}'.");
                MethodInfo initialize = interfaceType.GetMethod("Initialize") ?? throw new MissingMethodException(interfaceType.FullName, "Initialize");
                try
                {
                    initialize.Invoke(instance, null);
                }
                catch (TargetInvocationException exception) when (exception.InnerException is not null)
                {
                    throw new InvalidOperationException($"Mod initializer '{entryPointType.FullName}' failed.", exception.InnerException);
                }

                Log.Write($"Initialized mod entry point: {entryPointType.FullName}");
            }
        }
    }

    internal sealed class ModLoadContext(string mainAssemblyPath) : AssemblyLoadContext($"AlluLoader:{Path.GetFileNameWithoutExtension(mainAssemblyPath)}", isCollectible: false)
    {
        private readonly AssemblyDependencyResolver _resolver = new AssemblyDependencyResolver(mainAssemblyPath);
        private readonly string _modDirectory = Path.GetDirectoryName(mainAssemblyPath) ?? Paths.Mods;
        protected override Assembly? Load(AssemblyName assemblyName)
        {
            Assembly? sharedAssembly = Default.Assemblies.FirstOrDefault(assembly => AssemblyName.ReferenceMatchesDefinition(assemblyName, assembly.GetName()));
            if (sharedAssembly is not null) return sharedAssembly;
            string? assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath is null && !string.IsNullOrWhiteSpace(assemblyName.Name))
            {
                string candidate = Path.Combine(_modDirectory, $"{assemblyName.Name}.dll");
                if (File.Exists(candidate)) assemblyPath = candidate;
            }
            return assemblyPath is null ? null : LoadFromAssemblyPath(Path.GetFullPath(assemblyPath));
        }

        protected override nint LoadUnmanagedDll(string unmanagedDllName)
        {
            string? libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath is not null) return LoadUnmanagedDllFromPath(libraryPath);
            foreach (string fileName in GetNativeLibraryNames(unmanagedDllName))
            {
                string candidate = Path.Combine(_modDirectory, fileName);
                if (File.Exists(candidate)) return LoadUnmanagedDllFromPath(Path.GetFullPath(candidate));
            }
            return nint.Zero;
        }

        private static IEnumerable<string> GetNativeLibraryNames(string name)
        {
            yield return name;

            if (OperatingSystem.IsWindows() && !name.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
            {
                yield return $"{name}.dll";
            }
            else if (OperatingSystem.IsLinux() && !name.EndsWith(".so", StringComparison.Ordinal))
            {
                yield return $"lib{name}.so";
            }
            else if (OperatingSystem.IsMacOS() && !name.EndsWith(".dylib", StringComparison.Ordinal))
            {
                yield return $"lib{name}.dylib";
            }
        }
    }

    internal static class SharedLibraryResolver
    {
        public static Assembly? Resolve(
            AssemblyLoadContext context,
            AssemblyName assemblyName)
        {
            if (string.IsNullOrWhiteSpace(assemblyName.Name)) return null;
            string candidate = Path.GetFullPath(Path.Combine(Paths.Libraries, $"{assemblyName.Name}.dll"));
            if (!File.Exists(candidate)) return null;
            try
            {
                AssemblyName candidateName = AssemblyName.GetAssemblyName(candidate);
                if (!AssemblyName.ReferenceMatchesDefinition(assemblyName, candidateName))
                {
                    Log.Write($"Rejected shared library '{candidate}': identity '{candidateName.FullName}' does not match requested '{assemblyName.FullName}'.");
                    return null;
                }
                return context.LoadFromAssemblyPath(candidate);
            }
            catch (Exception exception)
            {
                Log.Write($"Could not resolve shared dependency '{assemblyName.FullName}': {exception}");
                return null;
            }
        }
    }
}