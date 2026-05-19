import {
  ComponentFixture,
  TestBed
} from '@angular/core/testing';

import { FooterComponent } from './footer.component';

import { By } from '@angular/platform-browser';

import { provideRouter } from '@angular/router';

import { ToastrService } from 'ngx-toastr';

describe('FooterComponent', () => {

  let component: FooterComponent;

  let fixture: ComponentFixture<FooterComponent>;

  let mockToastr: jasmine.SpyObj<ToastrService>;

  beforeEach(async () => {

    mockToastr = jasmine.createSpyObj(
      'ToastrService',
      ['success']
    );

    await TestBed.configureTestingModule({

      imports: [
        FooterComponent
      ],

      providers: [

        provideRouter([]),

        {
          provide: ToastrService,
          useValue: mockToastr
        }

      ]

    }).compileComponents();

    fixture = TestBed.createComponent(
      FooterComponent
    );

    component = fixture.componentInstance;

    fixture.detectChanges();

  });

  it('should create', () => {

    expect(component).toBeTruthy();

  });

  it('should display brand name', () => {

    const brandElement =
      fixture.debugElement.query(
        By.css('.footer-brand')
      );

    expect(brandElement).toBeTruthy();

    expect(
      brandElement.nativeElement.textContent
    ).toContain('ShopEZ');

  });

  it('should display copyright text', () => {

    const copyrightElement =
      fixture.debugElement.query(
        By.css('.footer-bottom')
      );

    expect(copyrightElement).toBeTruthy();

    expect(
      copyrightElement.nativeElement.textContent
    ).toContain('© 2026 ShopEZ');

  });

  it('should have shop links', () => {

    const shopLinks =
      fixture.debugElement.queryAll(
        By.css('.footer-links a')
      );

    expect(shopLinks.length)
      .toBeGreaterThan(0);

  });

  it('should call subscribe when newsletter button is clicked', () => {

    spyOn(component, 'subscribe');

    const emailInput =
      fixture.debugElement.query(
        By.css('input[type="email"]')
      ).nativeElement;

    emailInput.value = 'test@example.com';

    emailInput.dispatchEvent(
      new Event('input')
    );

    fixture.detectChanges();

    const subscribeButton =
      fixture.debugElement.query(
        By.css('.btn-primary.btn-sm')
      ).nativeElement;

    subscribeButton.click();

    expect(component.subscribe)
      .toHaveBeenCalledWith(
        'test@example.com'
      );

  });

  it('should show toastr when subscribe is called with email', () => {

    component.subscribe('test@example.com');

    expect(
      mockToastr.success
    ).toHaveBeenCalledWith(
      'Thanks for subscribing with test@example.com!'
    );

  });

  it('should not show toastr when subscribe is called with empty email', () => {

    component.subscribe('');

    expect(
      mockToastr.success
    ).not.toHaveBeenCalled();

  });

});