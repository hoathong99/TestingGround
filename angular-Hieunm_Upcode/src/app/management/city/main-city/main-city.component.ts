import { ChangeDetectionStrategy, Component, Injector, OnInit } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { MessageService } from 'primeng/api';
declare var google: any;

@Component({
  selector: 'app-main-city',
  templateUrl: './main-city.component.html',
  styleUrls: ['./main-city.component.css'],
  animations: [appModuleAnimation()],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MainCityComponent extends AppComponentBase implements OnInit {

  options: any;

  overlays: any[];
  basicData: any;

  basicOptions: any;

  constructor(injector: Injector,
    private messageService: MessageService
  ) {
    super(injector);
  }

  ngOnInit() {
    this.options = {
      center: { lat: 21.028511, lng: 105.804817 },
      zoom: 12
    };
    this.basicData = {
      labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
      datasets: [
        {
          label: 'My First dataset',
          backgroundColor: '#42A5F5',
          data: [65, 59, 80, 81, 56, 55, 40]
        },
        {
          label: 'My Second dataset',
          backgroundColor: '#FFA726',
          data: [28, 48, 40, 19, 86, 27, 90]
        }
      ]
    };


  }

  zoomIn() {
    this.basicOptions = {
      plugins: {
        legend: {
          labels: {
            color: '#ebedef'
          }
        }
      },
      scales: {
        x: {
          ticks: {
            color: '#ebedef'
          },
          grid: {
            color: 'rgba(255,255,255,0.2)'
          }
        },
        y: {
          ticks: {
            color: '#ebedef'
          },
          grid: {
            color: 'rgba(255,255,255,0.2)'
          }
        }
      }
    };
  }

}
