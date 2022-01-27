import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { SchoolComponent } from './school.component';

const routes: Routes = [
    {
        path: '',
        component: SchoolComponent,
        children: [

        ]
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class HotelRoutingModule { }
