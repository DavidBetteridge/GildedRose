using System;
using System.Collections.Generic;

namespace csharpcore
{
    public class GildedRose
    {
        private readonly IList<Item> _items;
        public GildedRose(IList<Item> Items)
        {
            _items = Items;
        }

        public void UpdateQuality()
        {
            foreach (var item in _items)
                UpdateItem(item);
        }

        private void UpdateItem(Item item)
        {
            item.Quality = item.Name switch
            {
                "Aged Brie" when item.Quality >= 50 => item.Quality,
                "Aged Brie" => item.Quality + 1,
                "Backstage passes to a TAFKAL80ETC concert" when item.SellIn > 10 => item.Quality + 1,
                "Backstage passes to a TAFKAL80ETC concert" when item.SellIn > 5 => item.Quality + 2,
                "Backstage passes to a TAFKAL80ETC concert" when item.SellIn > 0 => item.Quality + 3,
                "Backstage passes to a TAFKAL80ETC concert" => 0,
                "Sulfuras, Hand of Ragnaros" => item.Quality,
                "Conjured" => Math.Max(0, item.Quality - 2),
                _ when item.Quality <= 0 => item.Quality,
                _ when item.SellIn <= 0 => Math.Max(0, item.Quality - 2),
                _ => Math.Max(0, item.Quality - 1),
            };

            item.SellIn = item.Name switch
            {
                "Sulfuras, Hand of Ragnaros" => item.SellIn,
                _ => item.SellIn - 1,
            };
        }

    }
}
