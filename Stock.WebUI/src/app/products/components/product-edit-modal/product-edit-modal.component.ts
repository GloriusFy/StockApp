import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {ProductEditComponent} from '../product-edit/product-edit.component';
import {Product} from '../../models/product';

//TODO: utter crap, rework this as a service + component, like the confirmation dialog

@Component({
  selector: 'app-product-edit-modal',
  templateUrl: './product-edit-modal.component.html'
})
export class ProductEditModalComponent {

  @Input() selectedProductId: string;

}
