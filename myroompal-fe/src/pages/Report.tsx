import React, { useState } from 'react';
import { Button, Form } from 'react-bootstrap';
import axios from 'axios';
import { toast } from 'react-toastify';
import { config } from '../config';
import { TicketCreationData } from "../models/AdminPanel/TicketCreationData"; // Ensure this is imported properly

const Report = () => {
    const [issueType, setIssueType] = useState<string>(''); // Initialize as empty string
    const [description, setDescription] = useState('');
    const [loading, setLoading] = useState(false);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        const ticketCreationData: TicketCreationData = {
            issueType: issueType,
            description: description
        };

        try {
            setLoading(true);
            await createTicket(ticketCreationData);
        } catch (error) {
            console.error("Error submitting report:", error);
        } finally {
            setLoading(false);
        }
    };

    // Your method to create a ticket
    const createTicket = async (ticketCreationData: TicketCreationData): Promise<void> => {
        try {
            const response = await axios.post(config.apiBaseUrl + "/support", ticketCreationData);

            if (response.status === 200) {
                toast("Support ticket created successfully", { type: "success" });
            }
        } catch (error: any) {
            toast.error("Failed to create support ticket");
            console.error("Error creating support ticket:", error);
        }
    };

    return (
        <div className="container mt-4">
            <h1>Report an Issue</h1>
            <Form onSubmit={handleSubmit}>
                {/* Issue Type Dropdown */}
                <Form.Group controlId="ticketIssueType">
                    <Form.Label>Issue Type</Form.Label>
                    <Form.Control
                        as="select"
                        value={issueType}
                        onChange={(e) => setIssueType(e.target.value)}
                        required
                    >
                        <option value="">Select Issue Type</option>
                        <option value="Login">Login</option>
                        <option value="Payment">Payment</option>
                        <option value="Account">Account</option>
                        <option value="Renting">Renting</option>
                        <option value="Other">Other</option>
                    </Form.Control>
                </Form.Group>

                {/* Description Input */}
                <Form.Group controlId="ticketDescription" className="mt-3">
                    <Form.Label>Description</Form.Label>
                    <Form.Control
                        as="textarea"
                        rows={5}
                        placeholder="Describe the issue"
                        value={description}
                        onChange={(e) => setDescription(e.target.value)}
                        required
                    />
                </Form.Group>

                {/* Submit Button */}
                <Button variant="primary" type="submit" className="mt-3" disabled={loading}>
                    {loading ? 'Submitting...' : 'Submit Report'}
                </Button>
            </Form>
        </div>
    );
};

export default Report;
