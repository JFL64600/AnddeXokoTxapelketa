import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { initializeApp, provideFirebaseApp } from '@angular/fire/app';
import { getDatabase, provideDatabase } from '@angular/fire/database';

const firebaseConfig = {
  apiKey: "AIzaSyBJlpXCij3Hp0EdjjLuheYLWBm4Ms9i2Ug",
  authDomain: "andde-xoko-txapelketa.firebaseapp.com",
  databaseURL: "https://andde-xoko-txapelketa-default-rtdb.europe-west1.firebasedatabase.app",
  projectId: "andde-xoko-txapelketa",
  storageBucket: "andde-xoko-txapelketa.firebasestorage.app",
  messagingSenderId: "185833300257",
  appId: "1:185833300257:web:8012f62b4bb21b4edc5e60"
};

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideFirebaseApp(() => initializeApp(firebaseConfig)),
    provideDatabase(() => getDatabase()),
  ]
};
