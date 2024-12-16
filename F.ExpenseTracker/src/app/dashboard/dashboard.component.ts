import { Component, OnInit, AfterViewInit } from '@angular/core';
import Chart from 'chart.js/auto';
import ChartDataLabels from 'chartjs-plugin-datalabels';
import { ExpenseService } from '../expense.service';
import { NavbarComponent } from '../navbar/navbar.component';
import { FooterComponent } from '../footer/footer.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  templateUrl: './dashboard.component.html',
  imports: [CommonModule, FormsModule, NavbarComponent, FooterComponent],
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  chartData: any;
  chartInstance: any;
  isChartData: boolean = true;

  years: number[] = [];
  months: { value: number, name: string }[] = [
    { value: 1, name: 'January' },
    { value: 2, name: 'February' },
    { value: 3, name: 'March' },
    { value: 4, name: 'April' },
    { value: 5, name: 'May' },
    { value: 6, name: 'June' },
    { value: 7, name: 'July' },
    { value: 8, name: 'August' },
    { value: 9, name: 'September' },
    { value: 10, name: 'October' },
    { value: 11, name: 'November' },
    { value: 12, name: 'December' },
  ];
  selectedYear: number;
  selectedMonth: number;

  constructor(private expenseService: ExpenseService) {
    const currentYear = new Date().getFullYear();
    for (let year = currentYear; year >= currentYear - 10; year--) {
      this.years.push(year);
    }
    this.selectedYear = currentYear;
    this.selectedMonth = new Date().getMonth() + 1;
  }

  ngOnInit(): void {
    this.fetchData();
  }



  showChartByMonth(year: number, month: number): void {
    this.selectedYear = year;
    this.selectedMonth = month;
    this.fetchData();
  }

  fetchData(): void {
    this.expenseService.groupByExpense(this.selectedYear, this.selectedMonth).subscribe(
      data => {
        if (data && data.categories && data.categories.length > 0) {
          console.log(data);
          this.isChartData = true;
          this.updateChart(data);

        }
        else {
          this.isChartData = false;
        }

      },
      error => {
        console.error('Data fetch error:', error);
      }
    );
  }

  updateChart(data: any): void {
    if (this.chartInstance) {
      this.chartInstance.destroy();
    }

    const canvas = document.getElementById('chart') as HTMLCanvasElement;
    const ctx = canvas.getContext('2d');

    if (ctx) {
      this.chartInstance = new Chart(ctx, {
        type: 'pie',
        data: {
          labels: data.categories,
          datasets: [{
            label: 'Expense by Category',
            data: data.amounts,
            backgroundColor: [
              'rgba(255, 99, 132, 0.7)',
              'rgba(54, 162, 235, 0.7)',
              'rgba(255, 206, 86, 0.7)',
              'rgba(75, 192, 192, 0.7)',
              'rgba(153, 102, 255, 0.7)',
              'rgba(255, 159, 64, 0.7)'
            ],
            borderColor: [
              'rgba(255, 99, 132, 1)',
              'rgba(54, 162, 235, 1)',
              'rgba(255, 206, 86, 1)',
              'rgba(75, 192, 192, 1)',
              'rgba(153, 102, 255, 1)',
              'rgba(255, 159, 64, 1)'
            ],
            borderWidth: 1
          }]
        },
        options: {
          layout: {
            padding: {
              left: 0,
              right: 0,
              top: 0,
              bottom: 0
            }
          },
          responsive: true,
          plugins: {
            legend: {
              position: 'top',
            },
            title: {
              display: true,
              text: 'Expense by Category'
            },
            datalabels: {
              formatter: (value, ctx) => {
                if (typeof value !== 'number') {
                  return '';
                }
                const dataArray = ctx.dataset.data as number[];
                const sum = dataArray.reduce((a, b) => a + b, 0);
                const percentage = ((value / sum) * 100).toFixed(2) + "%";
                return percentage;
              },
              color: '#fff',
              font: {
                weight: 'bold'
              }
            }
          }
        }
      });
    } else {
      console.error('Canvas element not found');
    }
  }
}
