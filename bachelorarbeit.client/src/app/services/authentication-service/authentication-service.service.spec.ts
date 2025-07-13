import { TestBed } from '@angular/core/testing';
import { AuthService } from './authentication-service.service';

describe('AuthenticationServiceService', () => {
  let service: AuthService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuthService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
