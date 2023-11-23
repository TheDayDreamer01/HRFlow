﻿using AutoMapper;
using HRIS.Dtos.NotificationDto;
using HRIS.Exceptions;
using HRIS.Models;
using HRIS.Repositories.NotificationRepository;
using Microsoft.AspNetCore.JsonPatch;

namespace HRIS.Services.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly IMapper _mapper;
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(IMapper mapper, INotificationRepository notificationRepository)
        {
            _mapper = mapper;
            _notificationRepository = notificationRepository;
        }

        public async Task<GetNotificationDto> CreateNotification(Guid hrId, CreateNotificationDto request)
        {
            var hr = await _notificationRepository.GetUserById(hrId) ??
                throw new UserNotFoundException("Invalid email address. Please try again.");

            var notification = _mapper.Map<Notification>(request);
            notification.TeamId = hr.TeamId ?? Guid.Empty;

            var response = await _notificationRepository.CreateNotification(notification);
            if (!response)
            {
                throw new Exception("Failed to add new notification");
            }

            return _mapper.Map<GetNotificationDto>(notification);
        }

        public async Task<bool> DeleteNotification(Guid userId, Guid notificationId)
        {
            var user = await _notificationRepository.GetUserById(userId) ??
               throw new UserNotFoundException("Invalid email address. Please try again.");
            var notification = await _notificationRepository.GetNotification(user, notificationId) ??
                throw new NotificationNotFoundException("Notification does not exist. Please try again.");
            var response = await _notificationRepository.DeleteNotification(notification);
            if (!response)
            {
                throw new Exception("Failed to delete notification.");
            }

            return response;
        }

        public async Task<GetNotificationDto> GetNotification(Guid userId, Guid notificationId)
        {
            var user = await _notificationRepository.GetUserById(userId) ??
              throw new UserNotFoundException("Invalid email address. Please try again.");
            var notification = await _notificationRepository.GetNotification(user, notificationId) ??
                throw new NotificationNotFoundException("Notification does not exist. Please try again.");
            return _mapper.Map<GetNotificationDto>(notification);
        }

        public async Task<ICollection<GetNotificationDto>> GetNotifications(Guid userId)
        {
            var user = await _notificationRepository.GetUserById(userId) ??
             throw new UserNotFoundException("Invalid email address. Please try again.");
            var notifications = await _notificationRepository.GetNotifications(user);
            return _mapper.Map<ICollection<GetNotificationDto>>(notifications);
        }

        public async Task<bool> UpdateNotification(Guid hrId, Guid notificationId, JsonPatchDocument<Notification> request)
        {
            var hr = await _notificationRepository.GetUserById(hrId) ??
             throw new UserNotFoundException("Invalid email address. Please try again.");
            var notification = await _notificationRepository.GetNotification(hr, notificationId) ??
               throw new NotificationNotFoundException("Notification does not exist. Please try again.");

            return await _notificationRepository.UpdateNotification(notification, request);
        }

        public async Task<GetNotificationDto> UpdateNotifications(Guid hrId, Guid notificationId, UpdateNotificationDto request)
        {
            var hr = await _notificationRepository.GetUserById(hrId) ??
            throw new UserNotFoundException("Invalid email address. Please try again.");
            var notification = await _notificationRepository.GetNotification(hr, notificationId) ??
               throw new NotificationNotFoundException("Notification does not exist. Please try again.");

            var dbNotification = _mapper.Map<Notification>(request);
            dbNotification.Id = notificationId;
            dbNotification.TeamId = hr.TeamId ?? Guid.Empty;

            var isNotificationUpdated = await _notificationRepository.UpdateNotifications(notification, dbNotification);
            if (!isNotificationUpdated)
            {
                throw new Exception("Failed to update notification information.");
            }

            return _mapper.Map<GetNotificationDto>(dbNotification);
        }
    }
}
