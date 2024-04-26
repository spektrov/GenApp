import { Routes } from '@angular/router';
import { GenappPageComponent } from './pages/genapp-page/genapp-page.component';

export const routes: Routes = [
    {path: 'genapp', component: GenappPageComponent},
    {path: '', redirectTo: 'genapp', pathMatch: 'full'},
];
