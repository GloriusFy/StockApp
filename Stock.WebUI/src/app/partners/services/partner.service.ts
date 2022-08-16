import { Injectable } from '@angular/core';
import { PartnerListing } from '../models/partner-listing';
import { Observable } from 'rxjs';
import { PagedState } from 'app/core/http/models/paged-state';
import { PartnerDetails } from '../models/partner-details';
import { PartnerCreate } from '../models/partner-create';
import { PartnerUpdate } from '../models/partner-update';
import { ApiService } from 'app/core/http/services/api-service';
import { HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})

export class PartnerService {

  apiRouteName: string = 'partners';

  constructor(private apiService: ApiService) { }

  getAll(params: HttpParams): Observable<PagedState<PartnerListing>> {
      return this.apiService.get<PagedState<PartnerListing>>(this.apiRouteName, params);
  }

  get(id: string): Observable<PartnerDetails> {
      return this.apiService.get<PartnerDetails>(`${this.apiRouteName}/${id}`);
  }

  update(id: string, entity: PartnerUpdate): Observable<string> {
      return this.apiService.put<PartnerUpdate, string>(`${this.apiRouteName}/${id}`, entity, null, {
          successMessage: 'Changes saved.',
          failMessage: 'Saving changes failed.'
      });
  }

  create(entity: PartnerCreate): Observable<number> {
      return this.apiService.post<PartnerCreate, number>(this.apiRouteName, entity, null, {
        successMessage: 'New partner added.',
        failMessage: 'Partner creation failed.'
      });
  }

  delete(id: string): Observable<string> {
      return this.apiService.delete<string>(`${this.apiRouteName}/${id}`, null, {
        successMessage: 'Partner deleted.',
        failMessage: 'Partner deletion failed.'
      });
  }
}
