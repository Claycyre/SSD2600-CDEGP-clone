using System.Text.Json;
using SSD2600_CDEGP.Models;

namespace SSD2600_CDEGP.Services;

public class CartService(IHttpContextAccessor httpContextAccessor)
{
    private const string CartSessionKey = "Cart";

    private ISession Session => httpContextAccessor.HttpContext!.Session;

    public CartViewModel GetCart()
    {
        var json = Session.GetString(CartSessionKey);
        if (string.IsNullOrEmpty(json))
            return new CartViewModel();

        return JsonSerializer.Deserialize<CartViewModel>(json) ?? new CartViewModel();
    }

    public void AddItem(string id, string name, decimal price, int quantity, string? imageUrl)
    {
        var cart = GetCart();
        var existing = cart.Items.FirstOrDefault(i => i.Id == id);

        if (existing != null)
            existing.Quantity += quantity;
        else
            cart.Items.Add(new CartItem
            {
                Id = id,
                Name = name,
                Price = price,
                Quantity = quantity,
                ImageUrl = imageUrl
            });

        SaveCart(cart);
    }

    public void RemoveItem(string id)
    {
        var cart = GetCart();
        cart.Items.RemoveAll(i => i.Id == id);
        SaveCart(cart);
    }

    public void UpdateQuantity(string id, int quantity)
    {
        var cart = GetCart();
        var item = cart.Items.FirstOrDefault(i => i.Id == id);
        if (item == null) return;

        if (quantity <= 0)
            cart.Items.Remove(item);
        else
            item.Quantity = quantity;

        SaveCart(cart);
    }

    public void ClearCart()
    {
        Session.Remove(CartSessionKey);
    }

    private void SaveCart(CartViewModel cart)
    {
        Session.SetString(CartSessionKey, JsonSerializer.Serialize(cart));
    }
}
