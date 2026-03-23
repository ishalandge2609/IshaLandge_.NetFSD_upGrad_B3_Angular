/**
 * js/cart.js
 * Render cart page, handle qty changes and removal.
 */

$(function () {
  renderCartPage();
});

function renderCartPage() {
  var cart = Cart.get();
  var $container = $('#cart-items-container');
  var $summary   = $('#cart-summary-col');

  // Empty cart
  if (!cart.length) {
    $('#cart-items-col').removeClass('col-lg-8').addClass('col-12');
    $container.html(
      '<div class="empty-state">' +
        '<div class="empty-icon"><i class="bi bi-cart3"></i></div>' +
        '<h4>Your cart is empty</h4>' +
        '<p>Browse our products and add something you like!</p>' +
        '<a href="products.html" class="btn btn-primary">Browse Products</a>' +
      '</div>'
    );
    $summary.hide();
    return;
  }

  // Restore to 8-col when cart has items
  $('#cart-items-col').removeClass('col-12').addClass('col-lg-8');

  $summary.show();

  // Build rows
  var html = '';
  cart.forEach(function (item) {
    html +=
      '<div class="cart-row" data-id="' + item.id + '">' +
        '<img src="' + item.image + '" alt="' + item.name + '"/>' +
        '<div>' +
          '<div class="cart-item-name">' + item.name + '</div>' +
          '<div class="cart-item-cat">' + item.category + '</div>' +
          '<div class="qty-wrap">' +
            '<button class="qty-minus" data-id="' + item.id + '"><i class="bi bi-dash"></i></button>' +
            '<input class="qty-input" type="text" inputmode="numeric" pattern="[0-9]*" value="' + (item.qty || 1) + '" data-id="' + item.id + '"/>' +
            '<button class="qty-plus" data-id="' + item.id + '"><i class="bi bi-plus"></i></button>' +
          '</div>' +
        '</div>' +
        '<div>' +
          '<div class="cart-item-price">' + formatPrice(item.price * (item.qty || 1)) + '</div>' +
          '<button class="btn-remove remove-btn" data-id="' + item.id + '" title="Remove"><i class="bi bi-trash3"></i></button>' +
        '</div>' +
      '</div>';
  });
  $container.html('<div class="cart-table">' + html + '</div>');

  updateCartSummary(cart);
  bindCartEvents();
}

function updateCartSummary(cart) {
  var subtotal = Cart.calculateTotal();
  var shipping = subtotal > 3000 ? 0 : 49;
  var tax      = Math.round(subtotal * 0.18);
  var total    = subtotal + shipping + tax;

  $('#summary-subtotal').text(formatPrice(subtotal));
  $('#summary-shipping').text(shipping === 0 ? 'FREE' : formatPrice(shipping));
  $('#summary-tax').text(formatPrice(tax));
  $('#summary-total').text(formatPrice(total));
  $('#summary-count').text(Cart.count() + ' item(s)');

  if (subtotal < 3000) {
    $('#free-ship-msg').text('Add ' + formatPrice(3000 - subtotal) + ' more for FREE shipping!').show();
  } else {
    $('#free-ship-msg').text('You have free shipping!').show();
  }
}

function bindCartEvents() {
  // Remove item
  $(document).off('click.rm').on('click.rm', '.remove-btn', function () {
    var id  = parseInt($(this).data('id'));
    var $row = $(this).closest('.cart-row');
    $row.css({ opacity: 0, transition: 'opacity 0.2s ease' });
    setTimeout(function () {
      Cart.removeFromCart(id);
      renderCartPage();
      Toast.show('Item removed from cart');
    }, 220);
  });

  // Qty minus
  $(document).off('click.qm').on('click.qm', '.qty-minus', function () {
    var id   = parseInt($(this).data('id'));
    var cart = Cart.get();
    var item = null;
    for (var i = 0; i < cart.length; i++) { if (cart[i].id === id) { item = cart[i]; break; } }
    if (item && (item.qty || 1) > 1) {
      Cart.updateQty(id, (item.qty || 1) - 1);
      renderCartPage();
    }
  });

  // Qty plus
  $(document).off('click.qp').on('click.qp', '.qty-plus', function () {
    var id   = parseInt($(this).data('id'));
    var cart = Cart.get();
    var item = null;
    for (var i = 0; i < cart.length; i++) { if (cart[i].id === id) { item = cart[i]; break; } }
    if (item) {
      Cart.updateQty(id, (item.qty || 1) + 1);
      renderCartPage();
    }
  });

  // Manual qty input
  $(document).off('change.qi').on('change.qi', '.qty-input', function () {
    var id  = parseInt($(this).data('id'));
    var val = parseInt($(this).val()) || 1;
    Cart.updateQty(id, val);
    renderCartPage();
  });
}
