

export const calculateCartTotal = (products) => {

    return products.reduce(
        (total, product) => total + (product.price * product.quantity),
        0
    );
};