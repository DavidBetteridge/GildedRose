using System;
using System.Collections.Generic;

namespace csharpcore
{
    public class GildedRose
    {
        IList<Item> Items;
        public GildedRose(IList<Item> Items)
        {
            this.Items = Items;
        }

        public void UpdateQuality()
        {
            foreach (var item in Items)
            {
                UpdateItem(item);
            }
        }

        private void UpdateItem(Item item)
        {
            var qualityUpdaterForItem = QualityUpdaterForItem(item);
            var sellInUpdaterForItem = SellInUpdaterForItem(item);

            item.Quality = qualityUpdaterForItem(item.Quality);
            item.SellIn = sellInUpdaterForItem(item.SellIn);
        }

        private Func<int, int> SellInUpdaterForItem(Item item) =>
            item.Name switch
            {
                "Sulfuras, Hand of Ragnaros" => (Quality) => Quality,
                _ => (SellIn) => SellIn - 1,
            };

        private Func<int, int> QualityUpdaterForItem(Item item) =>
            item.Name switch
            {
                "Aged Brie" when item.Quality >= 50 => (Quality) => Quality,
                "Aged Brie" => (Quality) => Quality + 1,
                "Backstage passes to a TAFKAL80ETC concert" when item.SellIn > 10 => (Quality) => Quality + 1,
                "Backstage passes to a TAFKAL80ETC concert" when item.SellIn > 5 => (Quality) => Quality + 2,
                "Backstage passes to a TAFKAL80ETC concert" when item.SellIn > 0 => (Quality) => Quality + 3,
                "Backstage passes to a TAFKAL80ETC concert" => (Quality) => 0,
                "Sulfuras, Hand of Ragnaros" => (Quality) => Quality,
                _ when item.Quality <= 0 => (Quality) => Quality,
                "Conjured" => (Quality) => Math.Max(0, Quality - 2),
                _ when item.SellIn <= 0 => (Quality) => Math.Max(0, Quality - 2),
                _ => (Quality) => Math.Max(0, Quality - 1),
            };
    }
}
