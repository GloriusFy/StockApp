import { PartnerAddress } from "./partner-address";

export interface PartnerUpdate {
  id: string;
  name: string;

  address: PartnerAddress;
}
