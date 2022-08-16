import { PartnerAddress } from "./partner-address";

export interface PartnerDetails {
  id: string;
  name: string;

  createdAt: Date;
  lastModifiedAt: Date;

  address: PartnerAddress;
}
