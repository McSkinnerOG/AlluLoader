using AlluLoader.API;
using AlluLoader.API.Events;
using AlluLoader.API.Logging;
using Allumeria.EntitySystem.Components;

namespace ExampleAlluLoaderMod
{
    public class ExampleMod : IMod
    {
        public void Initialize()
        {
            Log.Write("ExampleMod started!");
            PlayerEvents.OnDeath += OnDeath;
            PlayerEvents.BeforeDamage += BeforeDamage;
            InventoryEvents.OnPlayerInventoryOpened += (s, e) =>
            {
                if (e.Chest != null)
                    Log.Write($"{e.Player.name} opened a chest.");
            };

            ChatEvents.OnCommand += (s, e) =>
            {
                if (e.Command == "mycommand")
                {
                    Chat.SendSystemMessage("You executed mycommand!");
                    e.Handled = true;
                }
            };
        }

        private static void OnDeath(object? sender, PlayerDeathEventArgs e)
        {
            Log.Write($"{e.Player.name} died at {e.Player.position}");
        }

        private static void BeforeDamage(object? sender, PlayerDamageEventArgs e)
        {
            Log.Write($"Damage received: Amount={e.DamageAmount}, Type={e.DamageType}");

            if (e.DamageType == HealthComponent.DamageType.Drowning)
            {
                Log.Write("Preventing drowning damage!");
                e.WasCancelled = true;
            }
        }
    }
}