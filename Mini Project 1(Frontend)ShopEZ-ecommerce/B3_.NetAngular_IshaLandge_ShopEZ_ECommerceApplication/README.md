# ShopEZ — Luxury Home Decor E-Commerce Application

## Overview

ShopEZ is a **frontend-only** e-commerce web application built for selling luxury home decor and lifestyle products. The app provides a complete shopping experience — from product discovery to order placement — running entirely in the browser with no backend server required.

---

## Tech Stack

| Layer | Technology |
|---|---|
| Markup | HTML5 |
| Styling | CSS3 + Bootstrap 5.3.2 |
| Icons | Bootstrap Icons 1.11.1 |
| Scripting | Vanilla JavaScript + jQuery 3.7.1 |
| Data | Static JSON (`data/products.json`) |
| Persistence | Browser `localStorage` |

---

## Project Structure

```
ShopEZ-E-Commerce Application(Frontend)/
├── index.html              # Home page ( nav hero, featured products, categories)
├── products.html           # All products with filter, search, sort ,nav, breadcrumbing
├── product-details.html    # Single product detail view
├── cart.html               # Shopping cart
├── checkout.html           # Checkout form + order confirmation
├── css/
│   └── styles.css          # Global custom styles
├── js/
│   ├── common.js           # Cart module, Toast, formatPrice helpers
│   ├── products.js         # Product loading, card builder, display
│   ├── cart.js             # Cart page rendering & interactions
│   └── checkout.js         # Checkout form, validation, order flow
├── data/
│   └── products.json       # Product catalogue (40+ items, 6 categories)
└── images/
    └── *.jpg               # Product images
```

---

## Features

- Browse 35+ home decor products across 6 categories
- Filter by category, search by name/description, sort by price
- Persistent shopping cart via `localStorage`
- Quantity management (add, update, remove)
- Order summary with subtotal, GST (18%), and shipping calculation
- Checkout form with client-side validation
- Order confirmation screen with unique Order ID
- Responsive design (mobile, tablet, desktop)
- Toast notifications for cart actions

---

## Running Locally

Because the app fetches `data/products.json` via AJAX, it must be served over HTTP (not opened directly as a file):

```bash


# Option 1— VS Code
Install the "Live Server" extension open index.html , then click "Go Live"

# Option 2— VS Code
open index.html , then right click and select open with live server 
Then open `http://localhost:8000` in your browser.

```
---

## Product Categories

Furniture · Lighting · Planters · Kitchen & Dining · Bathroom Decor · Decorative Items

---

## Notes

- No backend, database, or authentication required.
- Payment gateway is not integrated; Cash on Delivery is the only active method (UPI/Card shown as "Coming Soon").
- All order data is saved to `localStorage` for session persistence.

---

© 2026 ShopEZ. All rights reserved.
