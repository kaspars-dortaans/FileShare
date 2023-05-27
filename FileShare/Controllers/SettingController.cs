using AutoMapper;
using FileShare.Authorization;
using FileShare.Enums;
using FileShare.Helpers;
using FileShare.Models;
using FileShare.Services;
using FileShare.ViewModels.Setting;
using Microsoft.AspNetCore.Mvc;

namespace FileShare.Controllers;

[Authorize(Role.Admin)]
[ApiController]
[Route("api/[controller]/[action]")]
public class SettingController : ControllerBase
{
    private readonly ISettingsService _settingsService;
    private readonly IMapper _mapper;
    public SettingController(ISettingsService settingService, IMapper mapper)
    {
        _settingsService = settingService;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetSettings()
    {
        var settings = _settingsService.GetAll().ToList();
        return Ok(_mapper.Map<IEnumerable<SettingListItem>>(settings));
    }

    [HttpGet]
    public IActionResult EditSetting(int id)
    {
        var setting = _settingsService.FindByIdOrDefault(id) ?? throw new AppException("Setting with given Id was not found");
        var settingViewModel = _mapper.Map<SettingViewModel>(setting);
        return Ok(settingViewModel);
    }

    [HttpPost]
    public IActionResult EditSetting(SettingViewModel model)
    {
        var settingToUpdate = _settingsService.FindByIdOrDefault(model.Id) ?? throw new AppException("Setting with given Id was not found");
        _settingsService.Update(_mapper.Map<SettingViewModel, Setting>(model, settingToUpdate));
        return Ok();
    }

    [HttpDelete]
    public IActionResult ClearSetting(int id)
    {
        var settingToClear = _settingsService.FindByIdOrDefault(id) ?? throw new AppException("Setting with given Id was not found");
        settingToClear.Value = string.Empty;
        _settingsService.Update(settingToClear);
        return Ok();
    }

}
