import styled from 'styled-components';

export const DefaultButton = styled.button`
    background: #f0f0f0;
    border: none;
    border-radius: 4px;
    display: flex;
    align-items: center;
    gap: 4px;
    padding: 4px 6px;
    font-weight: bold;
    font-size: medium;

    &:hover {
        cursor: pointer;
    }

    &:focus {
        outline: none;
        background: #e0e0e0;
    }
`;

export const PrimaryButton = styled(DefaultButton)`
    background: #ffea31;
    &:focus {
        outline: none;
        background: #e6d32c;
    }
`;

export const IconButton = styled.img`
    &:hover {
        cursor: pointer;
    }
`;
