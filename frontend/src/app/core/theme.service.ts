import { Injectable, signal } from '@angular/core';

const STORAGE_KEY = 'overload-theme';
const DARK_CLASS = 'app-dark';

/** Switches between the dark (default) and light theme by toggling a class on <html>. */
@Injectable({ providedIn: 'root' })
export class ThemeService {
  readonly isDark = signal(true);

  constructor() {
    this.isDark.set(this.readStored() !== 'light');
    this.apply();
  }

  toggle(): void {
    this.isDark.update((dark) => !dark);
    this.apply();
    try {
      localStorage.setItem(STORAGE_KEY, this.isDark() ? 'dark' : 'light');
    } catch {
      // Storage can be unavailable (private mode); the theme just won't persist.
    }
  }

  private apply(): void {
    document.documentElement.classList.toggle(DARK_CLASS, this.isDark());
  }

  private readStored(): string | null {
    try {
      return localStorage.getItem(STORAGE_KEY);
    } catch {
      return null;
    }
  }
}
