class ErrorDetail {
    code: string;
    message: string;
    details: string;

    constructor(code: string, message: string, details: string) {
        this.code = code;
        this.message = message;
        this.details = details;
    }
}

export class BaseResponse<T> {
    isSuccess: boolean;
    isFailure: boolean;
    statusCode: number;
    error: ErrorDetail;
    processingTime: number;
    servicedBy: number;
    additionalDetails: any;
    operationId: string;
    apiVersion: string;
    value: T;
    handlerMethod: string;

    constructor(
        isSuccess: boolean,
        isFailure: boolean,
        statusCode: number,
        error: ErrorDetail,
        processingTime: number,
        servicedBy: number,
        additionalDetails: any,
        operationId: string,
        apiVersion: string,
        value: T,
        handlerMethod: string
    ) {
        this.isSuccess = isSuccess;
        this.isFailure = isFailure;
        this.statusCode = statusCode;
        this.error = new ErrorDetail(error.code, error.message, error.details);
        this.processingTime = processingTime;
        this.servicedBy = servicedBy;
        this.additionalDetails = additionalDetails;
        this.operationId = operationId;
        this.apiVersion = apiVersion;
        this.value = value;
        this.handlerMethod = handlerMethod;
    }
}