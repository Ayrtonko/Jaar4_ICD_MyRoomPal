import { TicketData } from "../models/AdminPanel/TicketData";
import { wait } from "@testing-library/user-event/dist/utils";
import { toast } from "react-toastify";
import { AdminUserSearchUser } from "../models/AdminPanel/AdminUserSearchUser";
import EnumUserStatus from "../models/EnumUserStatus";
import axios from "axios";
import { config } from "../config";
import {TicketCreationData} from "../models/AdminPanel/TicketCreationData";

class AdminPanelService {
    static async getTicketData(): Promise<TicketData[]> {
        try {
            const response = await axios.get(config.apiBaseUrl + "/support");

            if (response.status === 200) {
                toast("Support tickets fetched successfully", { type: "success" });
                return response.data;
            }
        } catch (error: any) {
            this.handleError(error, "Failed to fetch ticket data");
        }
        return [];
    }

    static async updateTicketStatus(selectedTickets: TicketData[], newStatus: string): Promise<void> {
        try {
            const response = await axios.put(config.apiBaseUrl + "/support", {
                supportTicketIds: selectedTickets.map(ticket => ticket.id),
                status: newStatus,
            });

            if (response.status === 200) {
                toast("Support tickets status updated successfully", { type: "success" });
            }
        } catch (error: any) {
            this.handleError(error, "Failed to update ticket status");
        }
    }

    static async createTicket(ticketCreationData: TicketCreationData): Promise<void> {
        try {
            const response = await axios.post(config.apiBaseUrl + "/support", ticketCreationData);

            if (response.status === 200) {
                toast("Support ticket created successfully", {type: "success"});
            }
        } catch (error: any) {
            this.handleError(error, "Failed to create support ticket");
        }
    }


    static async getUsersFromSearch(query: string): Promise<AdminUserSearchUser[]> {
        try {
            const response = await axios.get(config.apiBaseUrl + "/user/search", {
                params: { query },
            });

            if (response.status === 200) {
                toast("User search results fetched successfully", { type: "success" });
                return response.data;
            }
        } catch (error: any) {
            this.handleError(error, "Failed to load user search results");
        }
        return [];
    }

    static async changeUserStatus(user: AdminUserSearchUser, newStatus: EnumUserStatus): Promise<void> {
        try {
            const response = await axios.put(config.apiBaseUrl + "/user/status", {
                id: user.id,
                status: newStatus,
            });

            if (response.status === 200) {
                toast(`User ${user.name} status has been changed to: ${newStatus}`, {
                    type: "success",
                    position: "top-right",
                    pauseOnHover: true,
                    hideProgressBar: true,
                });
            }
        } catch (error: any) {
            this.handleError(error, `Failed to change status for user ${user.name}`);
        }
    }

    static async deleteUser(user: AdminUserSearchUser): Promise<void> {
        try {
            await wait(1000); // Simulated delay
            toast(`User ${user.name} has been deleted`, {
                type: "success",
                position: "top-right",
                pauseOnHover: true,
                hideProgressBar: true,
            });
        } catch (error: any) {
            this.handleError(error, `Failed to delete user ${user.name}`);
        }
    }

    private static handleError(error: any, defaultMessage: string): void {
        if (error.response) {
            // Server responded with a status other than 2xx
            toast(`Error ${error.response.status}: ${error.response.data || defaultMessage}`, { type: "error" });
        } else if (error.request) {
            // Request was made but no response received
            toast("No response received from the server", { type: "error" });
        } else {
            // Other errors (e.g., in setting up the request)
            toast(`Error: ${error.message || defaultMessage}`, { type: "error" });
        }
    }
}

export default AdminPanelService;
