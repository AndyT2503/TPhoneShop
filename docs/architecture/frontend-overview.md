# Frontend Overview

This frontend is built as an NX micro-frontend architecture with three independent apps:
- `auth`
- `commerce`
- `admin`

These apps are composed using Module Federation so they can load shared UI modules, common libraries, and application features at runtime while remaining deployable independently.

## Architecture

- NX workspace manages multiple front-end apps and shared libraries.
- `auth`, `commerce`, and `admin` are separate Micro Frontends.
- Module Federation connects them, enabling composition of shells and remote apps without bundling everything together in a single monolith.
- Shared libraries and UI components are centralized in common `libs` folders, reducing duplication and simplifying maintenance.

## Design System

- The frontend uses Tailwind CSS for utility-first styling.
- Design is built manually with Tailwind rather than using a component library.
- The codebase prefers `@apply` to reuse Tailwind utility patterns and keep styles concise.
- UI styling is organized into reusable style classes in shared CSS or SCSS files.

## State Management

- The workspace uses NgRx Signal Store as the main state management library.
- The implementation follows the latest Angular signal APIs.
- Reactive RxJS usage is minimized, with signals preferred for local and shared state flows.
- NgRx Signal Store enables a more modern, efficient store pattern with better integration into Angular's reactivity model.

## UX / Design Reference

- The frontend design is based on the Figma file:
  https://www.figma.com/make/of1iZY4PaZOq6YirCPn5Dp/Design-TPhoneShop-E-commerce-Site?t=NtlnpuAH4hYQPzsy-1

## Apps Overview

### Auth App

- Handles authentication flows for login, registration, password reset, and session management.
- Provides UI and forms for users to sign in, sign up, and recover accounts.
- Integrates with the backend Identity Service for auth operations.

### Commerce App

- Handles product browsing, catalog display, product detail, search, and checkout UX.
- Connects to CommerceService for product data and search integration.
- Uses Tailwind-based design system for responsive layouts and consistent visual presentation.

### Admin App

- Provides admin dashboards for product, category, and user management.
- Supports role and permission administration for commerce access control.
- Integrates with backend services for management operations and monitoring.

## Technical Notes

- Each app should expose only the modules needed by other apps through Module Federation.
- Shared UI and utility libraries should be loaded as federated shared modules, not duplicated across apps.
- Tailwind configuration should be centralized and shared across all apps to ensure consistent spacing, colors, and typography.
- Styles should favor `@apply` rules in shared CSS classes to reduce inline utility noise.
- NgRx Signal Store should manage app state while keeping component logic declarative and signal-driven.
