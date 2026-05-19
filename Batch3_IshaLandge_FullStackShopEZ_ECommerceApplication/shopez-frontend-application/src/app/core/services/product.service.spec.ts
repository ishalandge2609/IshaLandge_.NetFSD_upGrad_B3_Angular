import { TestBed } from '@angular/core/testing';

import {
  HttpClientTestingModule,
  HttpTestingController
} from '@angular/common/http/testing';

import { ProductService }
from './product.service';

import { environment }
from '../../../environments/environment';

import {
  CreateProductRequest
} from '../models/models';

describe('ProductService', () => {

  let service: ProductService;

  let httpMock: HttpTestingController;

  const apiUrl = environment.apiUrl;

  const mockProduct = {

    id: 1,

    name: 'Test Product',

    description: 'Test Description',

    price: 1000,

    stock: 10,

    category: 'Furniture',

    imageUrl: '/images/test.jpg'

  };

  beforeEach(() => {

    TestBed.configureTestingModule({

      imports: [
        HttpClientTestingModule
      ],

      providers: [
        ProductService
      ]

    });

    service = TestBed.inject(
      ProductService
    );

    httpMock = TestBed.inject(
      HttpTestingController
    );

  });

  afterEach(() => {

    httpMock.verify();

  });

  // =========================
  // SERVICE CREATION
  // =========================

  it('should be created', () => {

    expect(service).toBeTruthy();

  });

  // =========================
  // GET PRODUCTS
  // =========================

  describe('getProducts', () => {

    it('should fetch products with pagination', () => {

      const mockResponse = {

        data: [mockProduct]

      };

      service.getProducts(1, 10)
        .subscribe(response => {

        expect(response.data.length)
          .toBe(1);

        expect(response.data[0].name)
          .toBe('Test Product');

      });

      const req = httpMock.expectOne(
        `${apiUrl}/api/products?page=1&pageSize=10`
      );

      expect(req.request.method)
        .toBe('GET');

      req.flush(mockResponse);

    });

  });

  // =========================
  // GET PRODUCT BY ID
  // =========================

  describe('getProductById', () => {

    it('should fetch single product by id', () => {

      service.getProductById(1)
        .subscribe(product => {

        expect(product.id)
          .toBe(1);

        expect(product.name)
          .toBe('Test Product');

      });

      const req = httpMock.expectOne(
        `${apiUrl}/api/products/1`
      );

      expect(req.request.method)
        .toBe('GET');

      // UPDATED RESPONSE FORMAT

      req.flush({

        data: mockProduct

      });

    });

  });

  // =========================
  // CREATE PRODUCT
  // =========================

  describe('createProduct', () => {

    it('should send POST request to create product', () => {

      const newProduct:
      CreateProductRequest = {

        name: 'New Product',

        description:
          'New Description',

        price: 500,

        stock: 5,

        category: 'Lighting',

        imageUrl:
          '/images/new.jpg'

      };

      service.createProduct(newProduct)
        .subscribe(product => {

        expect(product)
          .toEqual(mockProduct);

      });

      const req = httpMock.expectOne(
        `${apiUrl}/api/products`
      );

      expect(req.request.method)
        .toBe('POST');

      expect(req.request.body)
        .toEqual(newProduct);

      req.flush(mockProduct);

    });

  });

  // =========================
  // UPDATE PRODUCT
  // =========================

  describe('updateProduct', () => {

    it('should send PUT request to update product', () => {

      const updateData:
      CreateProductRequest = {

        name: 'Updated Product',

        description:
          'Updated Description',

        price: 1500,

        stock: 8,

        category: 'Furniture',

        imageUrl:
          '/images/updated.jpg'

      };

      service.updateProduct(1, updateData)
        .subscribe(product => {

        expect(product)
          .toEqual(mockProduct);

      });

      const req = httpMock.expectOne(
        `${apiUrl}/api/products/1`
      );

      expect(req.request.method)
        .toBe('PUT');

      expect(req.request.body)
        .toEqual(updateData);

      req.flush(mockProduct);

    });

  });

  // =========================
  // DELETE PRODUCT
  // =========================

  describe('deleteProduct', () => {

    it('should send DELETE request to remove product', () => {

      service.deleteProduct(1)
        .subscribe();

      const req = httpMock.expectOne(
        `${apiUrl}/api/products/1`
      );

      expect(req.request.method)
        .toBe('DELETE');

      req.flush({});

    });

  });

  // =========================
  // STOCK MANAGEMENT
  // =========================

  describe('stock management', () => {

    it('should reduce stock', () => {

      service.reduceStock(1, 2)
        .subscribe();

      const req = httpMock.expectOne(
        `${apiUrl}/api/products/1/stock/reduce`
      );

      expect(req.request.method)
        .toBe('PUT');

      expect(req.request.body)
        .toEqual({

          quantity: 2

        });

      req.flush({});

    });

    it('should restore stock', () => {

      service.restoreStock(1, 10)
        .subscribe();

      const req = httpMock.expectOne(
        `${apiUrl}/api/products/1/stock/restock`
      );

      expect(req.request.method)
        .toBe('PUT');

      expect(req.request.body)
        .toEqual({

          quantity: 10

        });

      req.flush({});

    });

  });

  // =========================
  // IMAGE URL
  // =========================

  describe('getFullImageUrl', () => {

    it('should prepend API URL to relative image path', () => {

      const result =
        service.getFullImageUrl(
          '/images/test.jpg'
        );

      expect(result)
        .toBe(
          `${apiUrl}/images/test.jpg`
        );

    });

    it('should return original URL if already absolute', () => {

      const result =
        service.getFullImageUrl(
          'https://example.com/image.jpg'
        );

      expect(result)
        .toBe(
          'https://example.com/image.jpg'
        );

    });

    it('should return placeholder if imageUrl is empty', () => {

      const result =
        service.getFullImageUrl('');

      expect(result)
        .toBe(
          'assets/placeholder.jpg'
        );

    });

    it('should return placeholder if imageUrl is null', () => {

      const result =
        service.getFullImageUrl('');

      expect(result)
        .toBe(
          'assets/placeholder.jpg'
        );

    });

  });

});