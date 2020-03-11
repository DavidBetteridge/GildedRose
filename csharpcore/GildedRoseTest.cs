using Xunit;
using System.Collections.Generic;

namespace csharpcore
{
    public static class ItemName
    {
        public const string ANormalItem = "foo";
        public const string AgedBrie = "Aged Brie";
        public const string Sulfuras = "Sulfuras, Hand of Ragnaros";
        public const string BackstagePasses = "Backstage passes to a TAFKAL80ETC concert";
        public const string Conjured = "Conjured";
    }


    public class GildedRoseTest
    {
        [Fact(DisplayName = "Conjured items decrease in quality by 2 each day.")]
        public void ConjuredDecreasesByTwoEachTime()
        {
            const int APositiveValue = 2;

            var Items = new List<Item> { new Item { Name = ItemName.Conjured, SellIn = APositiveValue, Quality = 10 } };
            var app = new GildedRose(Items);
            app.UpdateQuality();

            Assert.Equal(8, Items[0].Quality);
        }


        [Fact(DisplayName ="Normal items decrease in quality by 1 each day.")]
        public void QualityDecreasesByOneEachTime()
        {
            const int APositiveValue = 2;

            var Items = new List<Item> { new Item { Name = ItemName.ANormalItem, SellIn = APositiveValue, Quality = 10 } };
            var app = new GildedRose(Items);
            app.UpdateQuality();

            Assert.Equal(9, Items[0].Quality);
        }

        [Theory(DisplayName = "The SellIn for Normal items decrease by 1 each day.")]
        [InlineData(3, 2)]
        [InlineData(1, 0)]
        [InlineData(0, -1)]
        public void SellInDecreasesByOneEachTime(int initialValue, int expectedValue)
        {
            const int AnyThing = 123;

            var Items = new List<Item> 
            { 
                new Item { Name = ItemName.ANormalItem, SellIn = initialValue, Quality = AnyThing },
                new Item { Name = ItemName.BackstagePasses, SellIn = initialValue, Quality = AnyThing },
                new Item { Name = ItemName.AgedBrie, SellIn = initialValue, Quality = AnyThing },
                new Item { Name = ItemName.Conjured, SellIn = initialValue, Quality = AnyThing }
            };
            var app = new GildedRose(Items);
            app.UpdateQuality();

            Assert.Equal(expectedValue, Items[0].SellIn);
            Assert.Equal(expectedValue, Items[1].SellIn);
            Assert.Equal(expectedValue, Items[2].SellIn);
            Assert.Equal(expectedValue, Items[3].SellIn);
        }

        [Theory(DisplayName = "Item quality is only decreased when above zero.")]
        [InlineData(1, 0)]
        [InlineData(0, 0)]
        [InlineData(-1, -1)]
        public void OnlyPositiveQualityIsDecreased(int startingQuality, int expectedQuality)
        {
            const int APositiveValue = 2;

            var Items = new List<Item> { new Item { Name = ItemName.ANormalItem, SellIn = APositiveValue, Quality = startingQuality } };
            var app = new GildedRose(Items);
            app.UpdateQuality();

            Assert.Equal(expectedQuality, Items[0].Quality);
        }

        [Fact(DisplayName = "Aged Brie items increases in quality by 1 each day.")]
        public void AgedBrieQualityIncreasesByOneEachTime()
        {
            const int APositiveValue = 2;

            var Items = new List<Item> { new Item { Name = ItemName.AgedBrie, SellIn = APositiveValue, Quality = 10 } };
            var app = new GildedRose(Items);
            app.UpdateQuality();

            Assert.Equal(11, Items[0].Quality);
        }

        [Theory(DisplayName = "Quality can never be more than 50")]
        [InlineData(49, 50)]
        [InlineData(50, 50)]
        [InlineData(150, 150)]
        public void QualityCannotBeMoreThan50(int initialValue, int expectedValue)
        {
            const int APositiveValue = 2;

            var Items = new List<Item> { new Item { Name = ItemName.AgedBrie, SellIn = APositiveValue, Quality = initialValue } };
            var app = new GildedRose(Items);
            app.UpdateQuality();

            Assert.Equal(expectedValue, Items[0].Quality);
        }

        [Theory(DisplayName = "Normal item quality degrades by 2 after sell by has passed")]
        [InlineData(0, 10, 8)]
        [InlineData(-1, 10, 8)]
        public void NormalItemQualityDecreasesByTwoAfterSellInPassed(int sellIn, int initialQuality, int expectedQuality)
        {
            var Items = new List<Item> { new Item { Name = ItemName.ANormalItem, SellIn = sellIn, Quality = initialQuality } };
            var app = new GildedRose(Items);
            app.UpdateQuality();

            Assert.Equal(expectedQuality, Items[0].Quality);
        }

        [Fact(DisplayName = "Normal item quality cannot degrade past 0 when sell by passed")]
        public void SellInCannotDropQualityToNegative()
        {
            var Items = new List<Item> { new Item { Name = ItemName.ANormalItem, SellIn = 0, Quality = 1 } };
            var app = new GildedRose(Items);
            app.UpdateQuality();

            Assert.Equal(0, Items[0].Quality);
        }

        [Theory(DisplayName = "The values for Sulfuras never change")]
        [InlineData(4, 5)]
        [InlineData(2, 1)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        [InlineData(-1, -2)]
        public void TheValuesForSulfurasNeverChange(int sellIn, int quality)
        {
            var Items = new List<Item> { new Item { Name = ItemName.Sulfuras, SellIn = sellIn, Quality = quality } };
            var app = new GildedRose(Items);
            app.UpdateQuality();

            Assert.Equal(sellIn, Items[0].SellIn);
            Assert.Equal(quality, Items[0].Quality);
        }

        [Theory(DisplayName = "Backstage Passes increases in quality by 1 each day when SellIn before 10 days")]
        [InlineData(11, 5, 6)]
        [InlineData(20, 5, 6)]
        public void BackstagePassesIncreaseInQualityWhenSellInGreaterThan10Days(int sellIn, int qualityValue, int expectedQuality)
        {
            var Items = new List<Item> { new Item { Name = ItemName.BackstagePasses, SellIn = sellIn, Quality = qualityValue } };
            var app = new GildedRose(Items);
            app.UpdateQuality();

            Assert.Equal(expectedQuality, Items[0].Quality);
        }

        [Theory(DisplayName = "Backstage Passes increases in quality by 2 each day when SellIn between 10 and 5 days")]
        [InlineData(10, 5, 7)]
        [InlineData(6, 5, 7)]
        public void BackstagePassesIncreaseInQualityWhenSellInBetween10DaysAnd5Days(int sellIn, int qualityValue, int expectedQuality)
        {
            var Items = new List<Item> { new Item { Name = ItemName.BackstagePasses, SellIn = sellIn, Quality = qualityValue } };
            var app = new GildedRose(Items);
            app.UpdateQuality();

            Assert.Equal(expectedQuality, Items[0].Quality);
        }

        [Theory(DisplayName = "Backstage Passes increases in quality by 3 each day when SellIn between 5 and 0 days")]
        [InlineData(5, 5, 8)]
        [InlineData(1, 5, 8)]
        public void BackstagePassesIncreaseInQualityWhenSellInBetween5DaysAnd0Days(int sellIn, int qualityValue, int expectedQuality)
        {
            var Items = new List<Item> { new Item { Name = ItemName.BackstagePasses, SellIn = sellIn, Quality = qualityValue } };
            var app = new GildedRose(Items);
            app.UpdateQuality();

            Assert.Equal(expectedQuality, Items[0].Quality);
        }

        [Theory(DisplayName = "Backstage Passes has zero quality after the concert")]
        [InlineData(0, 5, 0)]
        [InlineData(-2, 4, 0)]
        public void BackstageQualityDropsToZeroAfterTheConcert(int sellIn, int qualityValue, int expectedQuality)
        {
            var Items = new List<Item> { new Item { Name = ItemName.BackstagePasses, SellIn = sellIn, Quality = qualityValue } };
            var app = new GildedRose(Items);
            app.UpdateQuality();

            Assert.Equal(expectedQuality, Items[0].Quality);
        }
    }
}