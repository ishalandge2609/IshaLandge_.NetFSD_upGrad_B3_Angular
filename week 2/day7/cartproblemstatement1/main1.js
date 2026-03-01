

import { calculateCartTotal } from "./cartutility.js";

const cart = [
    { name: "Laptop", price: 50000, quantity: 1 },
    { name: "Mouse", price: 500, quantity: 2 },
    { name: "Keyboard", price: 1500, quantity: 1 }
];

console.log(" Invoice");
console.log("----------------------");

cart.map(item => {
    console.log(`${item.name} (${item.quantity}) : ₹${item.price * item.quantity}`);
});

const totalAmount = calculateCartTotal(cart);

console.log("----------------------");
console.log(`Total Amount : ₹${totalAmount}`);