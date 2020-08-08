import { IApiValidationError } from '../services/kanbanApiService';

export const normalizeErrorPropertyName = (propertyName: string) =>
    `${propertyName[0].toLowerCase()}${propertyName.substring(1)}`;

export const mapApiErrorsToValidationErrors = (
    apiErrors: IApiValidationError[]
) =>
    apiErrors.map((error) => ({
        path: normalizeErrorPropertyName(error.property),
        errors: [error.message],
    }));
