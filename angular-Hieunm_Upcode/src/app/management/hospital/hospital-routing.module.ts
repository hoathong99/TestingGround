import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { HospitalComponent } from './hospital.component';

const routes: Routes = [
    {
        path: '',
        component: HospitalComponent,
        children: [

        ]
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class HospitalRoutingModule { }
