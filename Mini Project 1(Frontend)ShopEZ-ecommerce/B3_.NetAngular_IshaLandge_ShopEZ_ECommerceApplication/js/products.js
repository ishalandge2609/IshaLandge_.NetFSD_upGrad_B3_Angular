/**
 * js/products.js
 * Load and display products from products.json
 */

// Load products from JSON file
function loadProducts(callback) {
  $.getJSON('data/products.json', function (products) {
    callback(products);
  }).fail(function () {
    console.error('Could not load data/products.json');
    console.info('Tip: Run with Live Server or python -m http.server');
  });
}

// Build one product card HTML
function buildProductCard(product) {
  return (
    '<div class="product-card fade-in" data-id="' + product.id + '">' +
      '<div class="card-img-wrap">' +
        '<img src="' + product.image + '" alt="' + product.name + '" loading="lazy"/>' +
        '<span class="card-category">' + product.category + '</span>' +
      '</div>' +
      '<div class="card-body">' +
        '<div class="card-title">' + product.name + '</div>' +
        '<div class="card-desc">' + product.description + '</div>' +
        '<div class="card-price">' + formatPrice(product.price) + ' <small>/unit</small></div>' +
        '<div class="card-actions">' +
          '<a href="product-details.html?id=' + product.id + '" class="btn-details">' +
            'Product Details' +
          '</a>' +
          '<button class="btn-addcart add-to-cart-btn" data-id="' + product.id + '">' +
            '<i class="bi bi-cart-plus me-1"></i>Add to Cart' +
          '</button>' +
        '</div>' +
      '</div>' +
    '</div>'
  );
}

// Render product list into a container
function displayProducts(products, containerId) {
  containerId = containerId || '#product-grid';
  var $grid = $(containerId);

  if (!products || products.length === 0) {
    $grid.html(
      '<div class="col-12">' +
        '<div class="empty-state">' +
          '<div class="empty-icon"><i class="bi bi-search"></i></div>' +
          '<h4>No products found</h4>' +
          '<p>Try a different category or search term.</p>' +
        '</div>' +
      '</div>'
    );
    return;
  }

  var html = '';
  products.forEach(function (product, i) {
    html +=
      '<div class="col-md-6 col-lg-3 mb-4" style="animation-delay:' + (i * 0.06) + 's">' +
        buildProductCard(product) +
      '</div>';
  });
  $grid.html(html);
}

// Bind Add to Cart buttons (call after displaying)
function bindAddToCartButtons(allProducts) {
  $(document).off('click.atc').on('click.atc', '.add-to-cart-btn', function (e) {
    e.preventDefault();
    e.stopPropagation();
    var id = parseInt($(this).data('id'));
    var product = null;
    for (var i = 0; i < allProducts.length; i++) {
      if (allProducts[i].id === id) { product = allProducts[i]; break; }
    }
    if (product) {
      Cart.addToCart(product, 1);
      var $btn = $(this);
      $btn.html('<i class="bi bi-check-lg me-1"></i>Added!');
      setTimeout(function () { $btn.html('<i class="bi bi-cart-plus me-1"></i>Add to Cart'); }, 1300);
    }
  });
}
