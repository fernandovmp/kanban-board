import React from 'react';
import { Modal } from '../../../components';
import { ButtonsWrapper, CancelButton, ConfirmButton, Header } from './styles';

interface IDeleteModalProps {
    onCancel(): void;
    onConfirm(): void;
}

export const DeleteModal: React.FC<IDeleteModalProps> = ({
    onCancel,
    onConfirm,
}) => {
    return (
        <Modal title={<Header>Delete the board?</Header>}>
            Are you sure that you want to delete the board?
            <ButtonsWrapper>
                <CancelButton onClick={onCancel}>Cancel</CancelButton>
                <ConfirmButton onClick={onConfirm}>Confirm</ConfirmButton>
            </ButtonsWrapper>
        </Modal>
    );
};
