import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { HomeComponent } from './home/home.component';
import { AboutComponent } from './about/about.component';
import { UsersComponent } from './users/users.component';
import { TenantsComponent } from './tenants/tenants.component';
import { RolesComponent } from 'app/roles/roles.component';
import { ChangePasswordComponent } from './users/change-password/change-password.component';
import { MessagerComponent } from './users/messager/messager.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: AppComponent,
                children: [
                    { path: 'home', component: HomeComponent, canActivate: [AppRouteGuard] },
                    {
                        path: 'users', component: UsersComponent, data: { permission: 'Pages.Users' }, canActivate: [AppRouteGuard]

                    },
                    { path: 'roles', component: RolesComponent, data: { permission: 'Pages.Roles' }, canActivate: [AppRouteGuard] },
                    { path: 'tenants', component: TenantsComponent, data: { permission: 'Pages.Tenants' }, canActivate: [AppRouteGuard] },
                    { path: 'about', component: AboutComponent, canActivate: [AppRouteGuard] },
                    {
                        path: 'update-password', component: ChangePasswordComponent, canActivate: [AppRouteGuard]
                    },
                    { path: 'messager', component: MessagerComponent, canActivate: [AppRouteGuard] },
                    {
                        path: 'buildings',
                        loadChildren: () => import('./management/building/building.module').then(m => m.BuildingModule)
                    },
                    {
                        path: 'hotels',
                        loadChildren: () => import('./management/hotel/hotel.module').then(m => m.HotelModule)
                    },
                    {
                        path: 'schools',
                        loadChildren: () => import('./management/school/school.module').then(m => m.SchoolModule)
                    },
                    {
                        path: 'hospitals',
                        loadChildren: () => import('./management/hospital/hospital.module').then(m => m.HospitalModule)
                    },
                    {
                        path: 'communities',
                        loadChildren: () => import('./management/community/community.module').then(m => m.CommunityModule)
                    },
                    {
                        path: 'cities',
                        loadChildren: () => import('./management/city/city.module').then(m => m.CityModule)
                    },
                ]
            }

        ])
    ],
    exports: [RouterModule]
})
export class AppRoutingModule { }
