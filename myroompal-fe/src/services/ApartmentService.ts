import axios, {Axios} from "axios";
import {config} from "../config";
import {toast} from "react-toastify";

export interface ApartmentData {
    id: number;
    roomName: string;
    rentPrice: number;
    imageLink: string;
}

interface Address {
    id: number
    streetName: string;
    city: string;
    postalCode: string;
    country: string;
}
export interface ApartmentDetailData {
    id: number;
    roomName: string;
    rentPrice: number;
    imageLink: string;
    address: Address;
    description: string;
    size: number;
}
export class ApartmentService {
    static async getApartments(): Promise<ApartmentData[]> {
        try {
            const response = await axios.get(config.apiBaseUrl + "/room/all");
            if (response.status === 200) {
                toast("Rooms fetched succesfully", { type: "success" });
                return response.data;
            }
            return [];
        } catch (error: any) {
            toast("Something went wrong fetching the rooms!", { type: "error" });
            return [];
        }
    }

    static async getApartmentById(id: string): Promise<ApartmentDetailData> {
        try {
            const response = await axios.get(config.apiBaseUrl + `/room/${id}`);
            if (response.status === 200) {
                toast("Room fetched succesfully", { type: "success" });
                return response.data;
            }
            return {} as ApartmentDetailData;
        } catch (error: any) {
            toast("Something went wrong fetching the room!", { type: "error" });
            return {} as ApartmentDetailData;
        }
    }
}
