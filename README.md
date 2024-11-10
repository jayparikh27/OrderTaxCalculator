Assumptions<br />
1.There would be one coupon per product between any start date and end date. No days are overlapping.<br />
2.All discounts are in integer percentage format. No flat dollar discount.<br />
3.Promotional discount applied after coupon discount. <br />
4.Only one promotional discount and coupon discount applied per product.<br />
5.Order will be placed for single client. There can not be multiple clients in single order.<br />
6.Product, productCategory, state, client ,promotion, coupon,tax Rate tables has all the required data in database. <br />


Sample Input API body Json.<br>
{
  "products": [
    {
      "name": "Standard Chair",
      "quantity": 2
    },
 {
      "name": "Luxury Sofa",
      "quantity": 2
    }
  ],
  "clientName": "Client GA",
  "orderDate": "2024-11-09T09:12:44.522Z"
}
