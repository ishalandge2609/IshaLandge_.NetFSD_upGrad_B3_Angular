/**
 * js/checkout.js
 * Render order summary and handle checkout form submission.
 */

$(function () {
  var cart = Cart.get();
  if (!cart.length) {
    window.location.href = 'cart.html';
    return;
  }
  renderCheckoutSummary(cart);
  bindCheckoutForm();
});

// Render order items + totals in sidebar
function renderCheckoutSummary(cart) {
  var html = '';
  cart.forEach(function (item) {
    html +=
      '<div style="display:flex;align-items:center;gap:12px;margin-bottom:14px">' +
        '<img src="' + item.image + '" alt="' + item.name + '" ' +
             'style="width:50px;height:44px;object-fit:cover;border-radius:7px;background:#f1f3f5"/>' +
        '<div style="flex:1">' +
          '<div style="font-size:13px;font-weight:600;color:#1a1a2e">' + item.name + '</div>' +
          '<div style="font-size:12px;color:#6c757d">Qty: ' + (item.qty || 1) + '</div>' +
        '</div>' +
        '<div style="font-size:13.5px;font-weight:700;white-space:nowrap">' +
          formatPrice(item.price * (item.qty || 1)) +
        '</div>' +
      '</div>';
  });
  $('#co-items').html(html);

  var subtotal = Cart.calculateTotal();
  var shipping = subtotal > 3000 ? 0 : 49;
  var tax      = Math.round(subtotal * 0.18);
  var total    = subtotal + shipping + tax;

  $('#co-subtotal').text(formatPrice(subtotal));
  $('#co-shipping').text(shipping === 0 ? 'FREE' : formatPrice(shipping));
  $('#co-tax').text(formatPrice(tax));
  $('#co-total').text(formatPrice(total));
}

// Validate form fields
function validateForm() {
  var valid = true;

  var fields = [
    { id: '#f-name',    min: 3,  err: 'Enter your full name.' },
    { id: '#f-email',   min: 5,  err: 'Enter a valid email address.' },
    { id: '#f-phone',   min: 10, err: 'Enter a valid 10-digit phone number.' },
    { id: '#f-address', min: 8,  err: 'Enter your full street address.' },
    { id: '#f-city',    min: 2,  err: 'Enter your city.' },
    { id: '#f-pin',     min: 6,  err: 'Enter a valid 6-digit PIN code.' }
  ];

  fields.forEach(function (f) {
    var $input = $(f.id);
    var val    = $input.val().trim();
    var $fb    = $input.next('.invalid-feedback');

    if (!val || val.length < f.min) {
      $input.addClass('is-invalid');
      $fb.text(f.err).show();
      valid = false;
    } else {
      $input.removeClass('is-invalid');
      $fb.hide();
    }
  });

  // Email format check
  var email = $('#f-email').val().trim();
  if (email && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
    $('#f-email').addClass('is-invalid').next('.invalid-feedback').text('Enter a valid email address.').show();
    valid = false;
  }

  // Phone digits only
  var phone = $('#f-phone').val().replace(/\D/g, '');
  if (phone.length < 10) {
    $('#f-phone').addClass('is-invalid').next('.invalid-feedback').text('Enter a valid 10-digit phone number.').show();
    valid = false;
  }

  return valid;
}

// Bind form submit
function bindCheckoutForm() {
  $('#checkout-form').on('submit', function (e) {
    e.preventDefault();

    if (!validateForm()) return;

    // Show loading
    var $btn = $('#place-order-btn');
    $btn.html('<span class="spinner-border spinner-border-sm me-2"></span>Processing...').prop('disabled', true);

    // Simulate API call
    setTimeout(function () {
      // Save order record
      var order = {
        id:       'ORD' + Date.now(),
        date:     new Date().toLocaleDateString('en-IN'),
        name:     $('#f-name').val(),
        email:    $('#f-email').val(),
        items:    Cart.get(),
        total:    Cart.calculateTotal()
      };
      localStorage.setItem('last_order', JSON.stringify(order));

      // Clear cart
      Cart.clear();

      // Hide form, show success
      $('#checkout-main').hide();
      $('#checkout-success').html(buildSuccessHTML(order)).show();
    }, 1800);
  });

  // Clear error state on typing
  $(document).on('input', '.form-control', function () {
    $(this).removeClass('is-invalid').next('.invalid-feedback').hide();
  });
}

function buildSuccessHTML(order) {
  return (
    '<div class="success-box fade-in">' +
      '<div class="success-circle"><i class="bi bi-check-lg"></i></div>' +
      '<h3>Order Placed!</h3>' +
      '<p>Thank you, <strong>' + order.name + '</strong>! ' +
         'Your order <strong>' + order.id + '</strong> has been confirmed.<br>' +
         'A confirmation will be sent to <strong>' + order.email + '</strong>.</p>' +
      '<div class="d-flex gap-3 justify-content-center flex-wrap">' +
        '<a href="index.html" class="btn btn-primary">Back to Home</a>' +
        '<a href="products.html" class="btn btn-outline-secondary">Shop More</a>' +
      '</div>' +
    '</div>'
  );
}
