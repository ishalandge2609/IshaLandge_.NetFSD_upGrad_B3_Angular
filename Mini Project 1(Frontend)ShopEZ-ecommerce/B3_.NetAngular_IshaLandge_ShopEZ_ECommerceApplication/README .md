# ShopEZ 🛋️

**Luxury Home Decor E-Commerce Application**

A browser-based, frontend-only e-commerce application for selling luxury home decor and lifestyle products. ShopEZ delivers a complete end-to-end shopping experience — from product discovery and filtering, through cart management, to order placement — running entirely in the user's browser with no backend server.

---

## Table of Contents

- [Features](#features)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Module Overview](#module-overview)
- [Data Models](#data-models)
- [User Flow](#user-flow)
- [Pricing Logic](#pricing-logic)
- [Responsive Design](#responsive-design)
- [Known Limitations](#known-limitations)
- [Planned Enhancements](#planned-enhancements)

---

## Features

- 🛍️ Browse 36+ products across 6 curated categories
- 🔍 Real-time search, category filtering, and price sorting
- 🛒 Persistent cart using `localStorage` (survives page refreshes)
- 📦 Simulated checkout with client-side form validation
- ✅ Order confirmation with generated Order ID
- 📱 Fully responsive — mobile, tablet, and desktop

---

## Tech Stack

| Layer       | Technology                        | Notes                              |
|-------------|-----------------------------------|------------------------------------|
| Markup      | HTML5                             | Semantic, accessible structure     |
| Styling     | CSS3 + Bootstrap 5.3.2            | Custom `styles.css` included       |
| Icons       | Bootstrap Icons v1.11.1           | Via CDN                            |
| Scripting   | JavaScript + jQuery 3.7.1         | ES5 compatible                     |
| Data        | Static JSON                       | `data/products.json` (40+ items)   |
| Persistence | `localStorage`                    | Cart state + last order record     |

> No build step, bundler, or transpiler — all files are served as-is.

---

## Project Structure

```
shopez/
├── index.html               # Home page — hero, featured products, category tiles
├── products.html            # Full product catalogue with filters and search
├── product-details.html     # Single product detail view
├── cart.html                # Shopping cart with quantity controls and order summary
├── checkout.html            # Checkout form + order confirmation screen
│
├── css/
│   └── styles.css           # Global custom stylesheet
│
├── js/
│   ├── common.js            # Shared Cart, Toast, and formatPrice utilities
│   ├── products.js          # Product loading, card builder, grid renderer
│   ├── cart.js              # Cart page renderer and event handlers
│   └── checkout.js          # Form validation, order submission, success screen
│
├── data/
│   └── products.json        # Product catalogue (40+ items)
│
└── images/                  # Product photos (JPG)
```

---

## Getting Started

Since ShopEZ has no build step, you can run it directly:

1. **Clone or download** the repository.
2. **Open `index.html`** in a modern browser.

> **Note:** Loading `products.json` requires a local server due to browser CORS restrictions on `file://` URLs. Use one of the following:

```bash
# Python 3
python -m http.server 8000

# Node.js (npx)
npx serve .

# VS Code
# Use the "Live Server" extension
```

Then navigate to `http://localhost:8000` in your browser.

---

## Module Overview

### `common.js` — Shared Utilities

Loaded first on every page. Provides global `Cart`, `Toast`, and `formatPrice`.

| Method | Description |
|---|---|
| `formatPrice(amount)` | Returns ₹-prefixed Indian locale price string |
| `Cart.get()` | Returns cart array from `localStorage` |
| `Cart.addToCart(product, qty)` | Merges qty into existing item or adds new item; shows toast |
| `Cart.removeFromCart(id)` | Removes item by ID and saves |
| `Cart.updateQty(id, qty)` | Updates quantity (min 1) for a matching item |
| `Cart.calculateTotal()` | Returns sum of `price × qty` for all cart items |
| `Cart.count()` | Returns total item units in the cart |
| `Cart.clear()` | Removes cart from `localStorage` and resets badge |
| `Toast.show(msg, duration?)` | Displays a toast notification (default 2.6 s) |

### `products.js` — Product Rendering

- `loadProducts(callback)` — Fetches `products.json` via `$.getJSON`
- `buildProductCard(product)` — Returns an HTML string for a product card
- `displayProducts(products, containerId)` — Injects cards into a grid container
- `bindAddToCartButtons(allProducts)` — Handles add-to-cart click events with jQuery delegation

### `cart.js` — Cart Page Logic

- `renderCartPage()` — Reads localStorage, renders item rows or empty state
- `updateCartSummary(cart)` — Calculates subtotal, shipping, GST, and total
- `bindCartEvents()` — Attaches handlers for quantity changes and item removal

### `checkout.js` — Checkout Logic

- `validateForm()` — Validates all fields; adds Bootstrap `.is-invalid` feedback
- `bindCheckoutForm()` — On submit: validates → spinner (1.8 s) → saves order → clears cart → shows success screen
- `buildSuccessHTML(order)` — Constructs the order confirmation HTML

---

## Data Models

### Product (`data/products.json`)

```json
{
  "id": 1,
  "name": "Product Name",
  "category": "Furniture",
  "description": "Short description",
  "price": 12999,
  "rating": 4.5,
  "image": "images/product.jpg"
}
```

Categories: `Furniture`, `Lighting`, `Planters`, `Kitchen & Dining`, `Bathroom Decor`, `Decorative Items`

### Cart Item (`localStorage["cart"]`)

```json
{
  "id": 1,
  "name": "Product Name",
  "category": "Furniture",
  "price": 12999,
  "image": "images/product.jpg",
  "qty": 2
}
```

### Order Record (`localStorage["last_order"]`)

```json
{
  "id": "ORD1711234567890",
  "date": "23/03/2026",
  "name": "Customer Name",
  "email": "customer@email.com",
  "items": [...],
  "total": 25998
}
```

---

## User Flow

```
Home Page  →  Product Catalogue  →  Product Detail
                                          ↓
                                    Add to Cart
                                          ↓
                                    Cart Review
                                          ↓
                                    Checkout Form
                                          ↓
                                  Order Confirmation
```

1. **Home** — Browse featured products and category tiles
2. **Catalogue** — Filter, search, and sort the full product range
3. **Product Detail** — Review specs, select quantity, add to cart
4. **Cart** — Adjust quantities, remove items, view live totals
5. **Checkout** — Enter contact, shipping, and payment info (COD)
6. **Confirmation** — Receive Order ID and confirmation details

---

## Pricing Logic

| Component | Value |
|---|---|
| Base price | From `products.json` (INR, no decimals) |
| Shipping | ₹49 flat; **FREE** when subtotal > ₹50,000 |
| GST | 18% on subtotal only (not on shipping) |
| Grand Total | Subtotal + Shipping + GST |

---

## Responsive Design

| Breakpoint | Layout |
|---|---|
| Mobile (< 576 px) | Single-column grid, collapsed navbar |
| Tablet (576–991 px) | Two-column grid, summary stacks below cart |
| Desktop (≥ 992 px) | Four-column grid, sidebar alongside cart items |

---

## Known Limitations

- **No backend** — Data is static; no user accounts, persistent order history, or inventory management
- **No real payments** — Only Cash on Delivery is functional; UPI/Card are stubbed as "Coming Soon"
- **Single-device cart** — `localStorage` is browser-specific; cart does not sync across devices
- **Rating field unused** — Stored in JSON but not rendered in the UI
- **No wishlist, comparison, or review functionality**

---

## Planned Enhancements

| Enhancement | Description |
|---|---|
| Backend API | REST/GraphQL API for real-time inventory and order management |
| User Authentication | Login/register with JWT; cart and order history per account |
| Payment Integration | Razorpay or Stripe for UPI, card, and net banking |
| Product Ratings & Reviews | Render star ratings; add review submission |
| Admin Dashboard | CRUD for products, orders, and categories |
| Wishlist | Save products to a favourites list |
| Search Autocomplete | Debounced suggestions as the user types |
| PWA / Service Worker | Offline support and home screen installation |

---

*© 2026 ShopEZ · All rights reserved*
