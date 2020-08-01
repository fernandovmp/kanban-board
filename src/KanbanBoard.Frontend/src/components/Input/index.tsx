import styled from 'styled-components';

export const Input = styled.input`
    height: 25px;
    border-radius: 4px;
    border: 1px solid black;
    padding: 0 6px;

    &:focus {
        outline: none;
        border-color: var(--primary);
    }
`;
