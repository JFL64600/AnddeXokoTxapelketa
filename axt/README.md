# Andde Xoko Txapelketa

A tournament management web application built with Angular and Firebase.

## Overview

This application allows you to manage tournaments, including player registration, group organization, match scheduling, and score tracking. It uses Firebase Realtime Database for data persistence and is hosted on Firebase Hosting.

## Tech Stack

- **Angular** 21.2.0 - Modern web framework
- **Angular Material** - UI component library
- **Firebase** - Backend and hosting
  - Realtime Database for data storage
  - Hosting (Europe-West1 region)
- **TypeScript** - Type-safe development
- **Vitest** - Unit testing framework

## Prerequisites

- Node.js and npm (npm 11.10.0 or higher recommended)
- Angular CLI (`npm install -g @angular/cli`)

## Getting Started

### Installation

```bash
npm ci
```

### Development Server

To start a local development server:

```bash
npm start
# or
ng serve
```

Navigate to `http://localhost:4200/`. The application will automatically reload when you modify source files.

### Building for Production

```bash
npm run build
```

Build artifacts will be stored in the `dist/` directory, optimized for production.

### Watch Mode

For continuous development with automatic rebuilding:

```bash
npm run watch
```

## Testing

Run unit tests with Vitest:

```bash
npm test
# or
ng test
```

## Code Quality

Lint your code:

```bash
npm run lint
```

## Deployment

The application is automatically deployed to Firebase Hosting via GitHub Actions:

- **Production**: Deploys on push to `master` branch
- **Preview**: Deploys on pull requests

## Project Structure

- `src/app/` - Main application components
  - `tournament/` - Tournament management component
- `src/libs/` - Core models and business logic
  - `tournament.ts` - Tournament model
  - `group.ts` - Group model
  - `player.ts` - Player model
  - `match.ts` - Match model
  - `rotation.ts` - Rotation logic
  - `score.ts` - Score tracking

## Additional Resources

- [Angular CLI Documentation](https://angular.dev/tools/cli)
- [Angular Material Components](https://material.angular.io/)
- [Firebase Documentation](https://firebase.google.com/docs)
