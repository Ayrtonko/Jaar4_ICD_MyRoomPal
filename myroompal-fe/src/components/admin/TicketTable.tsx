import TicketTableRow from "./TicketTableRow";
import {useEffect, useState} from "react";
import AdminPanelService from "../../services/AdminPanelService";
import {TicketData} from "../../models/AdminPanel/TicketData";
import Spinner from "../Spinner";
import EnumSpinnerSize from "../../models/EnumSpinnerSize";

function TicketTable() {
    const [initialData, setData] = useState<TicketData[]>();
    const [selectedTickets, setSelectedTickets] = useState<TicketData[]>([]);

    useEffect(() => {
        const fetchData = async () => {
            const result = await AdminPanelService.getTicketData();
            setData(result);
        };
        fetchData();
    }, []);

    const handleSelectTicket = (ticket: TicketData) => {
        setSelectedTickets((prevSelectedTickets: TicketData[]): TicketData[] => {
            // Check if the ticket is already in the selected tickets array
            if (prevSelectedTickets.some(t => t.id === ticket.id)) {
                // If the ticket is already selected (is in array), remove it from the array
                return prevSelectedTickets.filter(t => t.id !== ticket.id);
            } else {
                // Add ticket to array of not selected
                return [...prevSelectedTickets, ticket];
            }
        });
    };

    const handleChangeStatus = async (newStatus: string) => {
        await AdminPanelService.updateTicketStatus(selectedTickets, newStatus);
        setData((prevData) =>
            prevData?.map((ticket) =>
                selectedTickets.includes(ticket) ? {...ticket, status: newStatus} : ticket
            )
        );
        setSelectedTickets([]);
    };

    const selectAllTickets = () => {
        if (initialData) {
            if (selectedTickets.length === initialData.length) {
                setSelectedTickets([]);
            } else
            setSelectedTickets(initialData);
        }
    }

    return (
        <div className="w-full">
            {initialData ? (
                <>
                    {initialData.length === 0 ? (
                        <div className="alert alert-info mt-5" role="alert">
                            No tickets to display.
                        </div>
                    ) : (
                        <>
                            <div className="mb-2 d-flex gap-3 align-middle justify-content-end align-items-center">
                                <p className="mb-0">({selectedTickets.length}) tickets selected</p>
                                <div className="btn-group">
                                    <button type="button"
                                            className={`btn btn-primary dropdown-toggle` + (selectedTickets.length === 0 ? " disabled" : "")}
                                            data-bs-toggle="dropdown" aria-expanded="false">
                                        Select state
                                    </button>
                                    <ul className="dropdown-menu">
                                        <li><a className="dropdown-item" href="#"
                                               onClick={() => handleChangeStatus("New")}>New</a></li>
                                        <li><a className="dropdown-item" href="#"
                                               onClick={() => handleChangeStatus("Committed")}>Committed</a></li>
                                        <li><a className="dropdown-item" href="#"
                                               onClick={() => handleChangeStatus("Done")}>Done</a></li>
                                    </ul>
                                </div>
                            </div>
                            <div className="table-responsive" style={{maxHeight: "400px", overflowY: "auto"}}>
                                <table className="table table-default table-striped table-hover">
                                    <thead className="table-darkblue" style={{position: "sticky", top: 0, zIndex: 1}}>
                                    <tr className="header-taller">
                                        <th scope="col" className="col-1">
                                            <input className="form-check-input border-black"
                                                   type="checkbox"
                                                   onClick={selectAllTickets}
                                                   checked={selectedTickets.length === initialData.length}
                                                   readOnly={true}
                                            ></input>
                                        </th>
                                        <th scope="col" className="col-1">#</th>
                                        <th scope="col" className="col-2">Issue</th>
                                        <th scope="col" className="col-6">Description</th>
                                        <th scope="col" className="col-1">Status</th>
                                    </tr>
                                    </thead>
                                    <tbody>
                                    {initialData.map((ticket, index) => (
                                        <TicketTableRow key={ticket.id}
                                                        ticketinfo={ticket}
                                                        index={index + 1}
                                                        selectTicket={handleSelectTicket}
                                                        isSelected={selectedTickets.some((t) => t.id === ticket.id)}
                                        />
                                    ))}
                                    </tbody>
                                </table>
                            </div>
                        </>
                    )}
                </>
            ) : (
                <div className="d-flex justify-content-center mt-5">
                    <Spinner size={EnumSpinnerSize.LG}/>
                </div>
            )}
        </div>
    );
}

export default TicketTable;
