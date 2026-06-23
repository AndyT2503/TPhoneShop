import { AUTH_ROUTES } from '../routes/auth-routes';
import { UrlSegment, UrlMatchResult } from '@angular/router';

const AUTH_PATHS = Object.values(AUTH_ROUTES) as string[];
export function authMatcher(segments: UrlSegment[]): UrlMatchResult | null {
  const firstSegment = segments[0]?.path;
  return AUTH_PATHS.includes(firstSegment) ? { consumed: [] } : null;
}
