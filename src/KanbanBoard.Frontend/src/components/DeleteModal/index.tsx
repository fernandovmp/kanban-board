import React from 'react';
import { Modal } from '../Modal';
import { ButtonsWrapper, CancelButton, ConfirmButton, Header } from './styles';

interface IDeleteModalProps {
    onCancel(): void;
    onConfirm(): void;
    headerText?: string;
}

export const DeleteModal: React.FC<IDeleteModalProps> = ({
    onCancel,
    onConfirm,
    children,
    headerText,
}) => {
    return (
        <Modal title={<Header>{headerText}</Header>}>
            {children}
            <ButtonsWrapper>
                <CancelButton onClick={onCancel}>Cancel</CancelButton>
                <ConfirmButton onClick={onConfirm}>Confirm</ConfirmButton>
            </ButtonsWrapper>
        </Modal>
    );
};
