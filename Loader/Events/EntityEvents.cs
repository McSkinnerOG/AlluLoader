using Allumeria.ChunkManagement;
using Allumeria.EntitySystem;
using Allumeria.EntitySystem.Components;

namespace AlluLoader.API.Events;

public class EntityEventArgs(Entity entity) : EventArgs
{ public Entity Entity { get; } = entity; }

public class EntityWorldEventArgs(Entity entity, World world) : EntityEventArgs(entity)
{ public World World { get; } = world; }

public class EntityComponentEventArgs(Entity entity, EntityComponent component) : EntityEventArgs(entity)
{ public EntityComponent Component { get; } = component; }

public static class EntityEvents
{
    public static event EventHandler<EntityWorldEventArgs>? Removing;
    public static event EventHandler<EntityWorldEventArgs>? Removed;
    public static event EventHandler<EntityComponentEventArgs>? ComponentAdding;
    public static event EventHandler<EntityComponentEventArgs>? ComponentAdded;

    internal static void InvokeRemoving(EntityWorldEventArgs e) => Removing?.Invoke(null, e);
    internal static void InvokeRemoved(EntityWorldEventArgs e) => Removed?.Invoke(null, e);
    internal static void InvokeComponentAdding(EntityComponentEventArgs e) => ComponentAdding?.Invoke(null, e);
    internal static void InvokeComponentAdded(EntityComponentEventArgs e) => ComponentAdded?.Invoke(null, e);
}