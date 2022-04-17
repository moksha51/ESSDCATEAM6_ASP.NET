using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CATeam6.Models;
using CATeam6.DB;
namespace CATeam6.Models
{
    public class CreateOrder
    {
        private MyDBContext createOrderDB;
        public CreateOrder(Guid userId, MyDBContext dBContext)
        {
            createOrderDB = dBContext;
            UserId = userId;
        }

        public Guid UserId { get; set; }

        public void MakeOrder()
        {
            Dictionary<Products, int> orderDetail = FindProducts();
            NewOrder(orderDetail);
            List<Cart> todelete = createOrderDB.Carts.Where(x => x.UserId.Id == UserId).ToList();
            foreach (Cart c in todelete) {
                createOrderDB.Remove(c);
            }
            createOrderDB.SaveChanges();
        }
        
        private Dictionary<Products, int> FindProducts()
        {
            List<Cart> cart = createOrderDB.Carts.Where(x => x.UserId.Id == this.UserId).ToList(); //Find all CartItem of user
            Dictionary<Products, int> productDetail = new Dictionary<Products, int>(); //new dict to store product with its quantity
            foreach (var item in cart)
            {
                productDetail.Add(item.Product, item.Quantity);
            }
            return productDetail;
        }
        private void NewOrder(Dictionary<Products, int> productDetail)
        {
            Orders newOrder = new Orders();
            newOrder.UserId = UserId;
            createOrderDB.Add(newOrder);

            foreach (var item in productDetail) // for each key value pair in the dict(product,quantity)
            {
                for (int i = 0; i < item.Value; i++) //loop quantity time to add all the products in his cart to the order
                {
                    createOrderDB.Add(new OrderDetails
                    {
                        ProductId = item.Key.Id,
                        OrdersId = newOrder.Id
                    });
                }
            }
            createOrderDB.SaveChanges();
        }
    }
}
