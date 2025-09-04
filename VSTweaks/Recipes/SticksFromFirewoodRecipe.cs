using System.Collections.Generic;
using Vintagestory.API.Common;

namespace VSTweaks.Recipes {
    internal sealed class SticksFromFirewoodRecipe : GridRecipe {
        public SticksFromFirewoodRecipe(IWorldAccessor world) {
            Name = "vstweaks.recipes.grid.stick_from_firewood";
            IngredientPattern = "T,F";
            Height = 1;
            Width = 2;
            Shapeless = true;

            var saw = new CraftingRecipeIngredient { Type = EnumItemClass.Item, Code = "game:saw-*", IsTool = true };
            var firewood = new CraftingRecipeIngredient { Type = EnumItemClass.Item, Code = "game:firewood" };

            Ingredients = new Dictionary<string, CraftingRecipeIngredient> { { "T", saw }, { "F", firewood } };
            Output = new CraftingRecipeIngredient { Type = EnumItemClass.Item, Code = "game:stick", Quantity = 3 };

            ResolveIngredients(world);
        }
    }
}
