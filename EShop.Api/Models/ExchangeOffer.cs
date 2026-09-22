namespace EShop.Api.Models;

public class ExchangeOffer
{
    public Item OfferedItem {get; set;}
    public Item WantedItem {get; set;}

    public ExchangeOffer (Item wantedItem, Item offeredItem)
    {
        OfferedItem = offeredItem;
        WantedItem = wantedItem;
    }
}