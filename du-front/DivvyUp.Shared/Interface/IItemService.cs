using DivvyUp_Shared.Dto;

namespace DivvyUp_Shared.Interface
{
    public interface IItemService
    {
        Task AddItem(ItemDto item);
        Task EditItem(ItemDto item);
        Task RemoveItem(int itemId);
        Task SetSettledItem(int itemId, bool isSettled);
        Task SetCompensationPriceItem(int itemId, double compensationPrice);
        Task<ItemDto> GetItem(int itemId);
        Task<List<ItemDto>> GetItems(int receiptId);
    }
}
