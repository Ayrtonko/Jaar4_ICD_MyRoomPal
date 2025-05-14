import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { ApartmentData, ApartmentService } from "../services/ApartmentService";
import "bootstrap/dist/css/bootstrap.min.css";
import Spinner from "../components/Spinner";
import EnumSpinnerSize from "../models/EnumSpinnerSize";

const ApartmentGallery: React.FC = () => {
    const [apartments, setApartments] = useState<ApartmentData[]>([]);
    const [filteredApartments, setFilteredApartments] = useState<ApartmentData[]>([]);
    const [loading, setLoading] = useState(true);
    const [searchTerm, setSearchTerm] = useState("");
    const [currentPage, setCurrentPage] = useState(1);
    const itemsPerPage = 12;

    const navigate = useNavigate();

    useEffect(() => {
        const fetchData = async () => {
            const data = await ApartmentService.getApartments();
            setApartments(data);
            setFilteredApartments(data);
            setLoading(false);
        };

        fetchData();
    }, []);

    useEffect(() => {
        const results = apartments.filter((apartment) =>
            apartment.roomName.toLowerCase().includes(searchTerm.toLowerCase())
        );
        setFilteredApartments(results);
        setCurrentPage(1); // Reset naar de eerste pagina bij zoeken
    }, [searchTerm, apartments]);

    const indexOfLastItem = currentPage * itemsPerPage;
    const indexOfFirstItem = indexOfLastItem - itemsPerPage;
    const currentItems = filteredApartments.slice(indexOfFirstItem, indexOfLastItem);

    const totalPages = Math.ceil(filteredApartments.length / itemsPerPage);

    return (
        <div className="container mt-4">
            <div className="d-flex justify-content-between align-items-center mb-4">
                <h2 className="fw-bold">Apartment Gallery</h2>
                <div className="d-flex align-items-center">
                    <input
                        type="text"
                        className="form-control form-control-sm me-2"
                        placeholder="Search by title..."
                        value={searchTerm}
                        onChange={(e) => setSearchTerm(e.target.value)}
                        style={{ maxWidth: "200px" }}
                    />
                </div>
            </div>

            <div className="row g-3">
                {loading ?
                    <div className="d-flex justify-content-center mt-5">
                        <Spinner size={EnumSpinnerSize.LG}/>
                    </div> :
                    currentItems.map((apartment) => (
                        <div
                            className="col-6 col-md-4 col-lg-3"
                            key={apartment.id}
                            onClick={() => navigate(`/room/${apartment.id}`)}
                            style={{cursor: "pointer"}}
                        >
                            <div className="card shadow-sm h-100">
                                <img
                                    src={apartment.imageLink}
                                    alt={apartment.roomName}
                                    className="card-img-top"
                                    style={{height: "180px", objectFit: "cover"}}
                                />
                                <div className="card-body text-center">
                                    <h6 className="card-title fw-semibold">{apartment.roomName}</h6>
                                    <p className="card-text text-muted">{apartment.rentPrice} € / month</p>
                                </div>
                            </div>
                        </div>
                    ))}
            </div>

            <nav className="d-flex justify-content-center align-items-center mt-4">
                <ul className="pagination pagination-sm">
                    {/* Previous Button */}
                    <li className={`page-item ${currentPage === 1 ? "disabled" : ""}`}>
                        <button
                            className="page-link"
                            onClick={() => currentPage > 1 && setCurrentPage(currentPage - 1)}
                        >
                            Previous
                        </button>
                    </li>

                    {/* Page Numbers */}
                    {Array.from({ length: totalPages }, (_, i) => i + 1).map((page) => (
                        <li key={page} className={`page-item ${currentPage === page ? "active" : ""}`}>
                            <button className="page-link" onClick={() => setCurrentPage(page)}>
                                {page}
                            </button>
                        </li>
                    ))}

                    {/* Next Button */}
                    <li className={`page-item ${currentPage === totalPages ? "disabled" : ""}`}>
                        <button
                            className="page-link"
                            onClick={() => currentPage < totalPages && setCurrentPage(currentPage + 1)}
                        >
                            Next
                        </button>
                    </li>
                </ul>
            </nav>
        </div>
    );
};

export default ApartmentGallery;
