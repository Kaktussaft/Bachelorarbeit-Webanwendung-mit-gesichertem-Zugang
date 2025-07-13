import { TestBed } from '@angular/core/testing';
import { CSRFInterceptor } from './csrf-interceptor.service';

describe('CsrfInterceptorService', () => {
  let service: CSRFInterceptor;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CSRFInterceptor);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
