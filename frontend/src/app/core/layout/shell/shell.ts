import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { ThemeService } from '../../theme.service';

/** Page frame: header with theme toggle, the routed screen, and the bottom tab bar. */
@Component({
  selector: 'app-shell',
  imports: [RouterOutlet, RouterLink, RouterLinkActive, ButtonModule],
  templateUrl: './shell.html',
  styleUrl: './shell.scss',
})
export class Shell {
  protected readonly theme = inject(ThemeService);

  protected readonly tabs = [
    { path: '/log', label: 'Log' },
    { path: '/history', label: 'History' },
    { path: '/progress', label: 'Progress' },
  ];
}
