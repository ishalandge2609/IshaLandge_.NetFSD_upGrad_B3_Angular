/**
 * js/common.js
 * Shared helpers: Cart, Toast, formatPrice
 * Load this FIRST on every page.
 */

// ── Price formatter ─────────────────────────────────────────────
function formatPrice(amount) {
  return '₹' + Number(amount).toLocaleString('en-IN');
}

// ── Cart Module ─────────────────────────────────────────────────
var Cart = {
  KEY: 'cart',

  // Get cart array from LocalStorage
  get: function () {
    return JSON.parse(localStorage.getItem(this.KEY)) || [];
  },

  // Save cart array to LocalStorage
  save: function (cart) {
    localStorage.setItem(this.KEY, JSON.stringify(cart));
    this.updateBadge();
  },

  // Add product to cart (with quantity)
  addToCart: function (product, qty) {
    qty = qty || 1;
    var cart = this.get();
    var existing = null;
    for (var i = 0; i < cart.length; i++) {
      if (cart[i].id === product.id) { existing = cart[i]; break; }
    }
    if (existing) {
      existing.qty = (existing.qty || 1) + qty;
    } else {
      var item = {
        id:          product.id,
        name:        product.name,
        category:    product.category,
        price:       product.price,
        image:       product.image,
        qty:         qty
      };
      cart.push(item);
    }
    this.save(cart);
    Toast.show(product.name + ' added to cart');
  },

  // Remove product by id
  removeFromCart: function (productId) {
    var cart = this.get().filter(function (i) { return i.id !== productId; });
    this.save(cart);
  },

  // Update quantity
  updateQty: function (productId, qty) {
    var cart = this.get();
    for (var i = 0; i < cart.length; i++) {
      if (cart[i].id === productId) { cart[i].qty = Math.max(1, qty); break; }
    }
    this.save(cart);
  },

  // Calculate total price
  calculateTotal: function () {
    var cart = this.get();
    var total = 0;
    cart.forEach(function (item) { total += item.price * (item.qty || 1); });
    return total;
  },

  // Total item count
  count: function () {
    var cart = this.get();
    var n = 0;
    cart.forEach(function (i) { n += (i.qty || 1); });
    return n;
  },

  // Clear entire cart
  clear: function () {
    localStorage.removeItem(this.KEY);
    this.updateBadge();
  },

  // Update navbar badge
  updateBadge: function () {
    var n = this.count();
    $('#cart-count').text(n);
    $('#cart-count').toggle(n > 0);
  }
};

// ── Toast notification ──────────────────────────────────────────
var Toast = {
  _timer: null,
  show: function (msg, duration) {
    duration = duration || 2600;
    if (!this._el) {
      this._el = $('<div class="shopez-toast"></div>').appendTo('body');
    }
    clearTimeout(this._timer);
    this._el.text(msg).addClass('show');
    var self = this;
    this._timer = setTimeout(function () {
      self._el.removeClass('show');
    }, duration);
  }
};

// ── Init on every page ──────────────────────────────────────────
$(function () {
  // Update cart badge
  Cart.updateBadge();

  // Highlight active nav link
  var page = window.location.pathname.split('/').pop() || 'index.html';
  $('.nav-link').each(function () {
    if ($(this).attr('href') === page) $(this).addClass('active');
  });
});
