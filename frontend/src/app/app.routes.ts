import { Routes } from '@angular/router';
import { Shell } from '@core/layout/shell/shell';

export const routes: Routes = [
  {
    path: '',
    component: Shell,
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'log' },
      { path: 'log', loadChildren: () => import('@features/log/log.routes').then((m) => m.LOG_ROUTES) },
      {
        path: 'history',
        loadChildren: () => import('@features/history/history.routes').then((m) => m.HISTORY_ROUTES),
      },
      {
        path: 'progress',
        loadChildren: () => import('@features/progress/progress.routes').then((m) => m.PROGRESS_ROUTES),
      },
    ],
  },
  { path: '**', redirectTo: 'log' },
];
