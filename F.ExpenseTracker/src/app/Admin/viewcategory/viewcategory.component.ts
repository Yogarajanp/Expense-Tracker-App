import { Component, ViewChild } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { CategoryService } from '../../category.service';
import { NavbarComponent } from '../../navbar/navbar.component';
import { AddcategoryComponent } from '../addcategory/addcategory.component';

@Component({
  selector: 'app-viewcategory',
  standalone: true,
  imports: [MatTableModule, MatButtonModule, MatIconModule, NavbarComponent, MatPaginator],
  templateUrl: './viewcategory.component.html',
  styleUrl: './viewcategory.component.css'
})
export class ViewcategoryComponent {
  displayedColumns: string[] = ['S.No', 'Category Type', 'Action'];
  dataSource = new MatTableDataSource<any>();
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(private categoryService: CategoryService, public dialog: MatDialog) { }

  ngOnInit(): void {
    this.loadRecords();


  }

  ngAfterViewInit(): void {
    console.log(this.paginator);
    this.paginator.pageSize = 5;
    this.dataSource.paginator = this.paginator;
  }
  
  loadRecords() {

    this.categoryService.getAllCategories()
      .subscribe(
        data => {

          this.dataSource.data = data;

        },
        error => {

          console.error('data fetch error:', error);

        }
      );
  }



  editCategory(category: any) {

  }
  removeCategory(CategoryId: any) {

  }
  async newCategory() {
    const dialogRef = this.dialog.open(AddcategoryComponent, {
      width: '1000px'
    });
    dialogRef.componentInstance.categoryAdded.subscribe(() => {
      this.loadRecords();
    })
  }

}
